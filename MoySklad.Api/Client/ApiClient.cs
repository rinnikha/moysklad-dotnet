using System.Data.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MoySklad.Api.Exceptions;
using MoySklad.Api.Utils;

namespace MoySklad.Api.Client;

public class ApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly MoySkladConfig _config;
    private readonly ILogger<ApiClient>? _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(MoySkladConfig config, ILogger<ApiClient>? logger = null)
    {
        _config = config;
        _logger = logger;

        if (string.IsNullOrEmpty(config.Token))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(config.Token));
        }

        // This handles gzip compressed responses automatically
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip,
            AllowAutoRedirect = false,
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(config.BaseUrl),
            Timeout = config.Timeout
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new MoySkladDateTimeConverter() // Add custom DateTime converter
            }
        };

        if (_config.Debug)
        {
            _logger?.LogDebug("ApiClient initialized");
            _logger?.LogDebug("  BaseUrl: {BaseUrl}", _config.BaseUrl);
            _logger?.LogDebug("  Host: {Host}", _httpClient.BaseAddress.Host);
            _logger?.LogDebug("  Timeout: {Timeout}", _config.Timeout.TotalSeconds);
            _logger?.LogDebug("  UserTimeZone: {TimeZone}",
                config.UserTimeZone?.DisplayName ?? "UTC (default)");
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, parameters);
        return await ExecuteWithRetryAsync(async () =>
        {
            using var request = CreateRequest(HttpMethod.Get, url);

            if (_config.Debug)
            {
                _logger?.LogDebug("GET request to {Url}", request.RequestUri);
            }

            using var response = await SendWithRedirectAsync(request, cancellationToken);

            if (_config.Debug)
            {
                _logger?.LogDebug("Response status code: {StatusCode}", response.StatusCode);
            }

            await HandleResponseAsync(response);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }, cancellationToken);
    }

    /// <summary>
    /// Fetches a resource by its absolute URL (e.g. the href from a ListEntity.Meta).
    /// The base URL prefix is stripped automatically so the underlying HttpClient can resolve it.
    /// </summary>
    public async Task<T?> GetByHrefAsync<T>(string absoluteHref, CancellationToken cancellationToken = default)
    {
        // Strip the base URL so we pass only the relative path to GetAsync
        var baseUrl = _config.BaseUrl.TrimEnd('/') + "/";
        var relative = absoluteHref.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase)
            ? absoluteHref.Substring(baseUrl.Length)
            : absoluteHref;

        return await GetAsync<T>(relative, cancellationToken: cancellationToken);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object? data = null,
        Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, parameters);
        return await ExecuteWithRetryAsync(async () =>
        {
            using var request = CreateRequest(HttpMethod.Post, url, data);

            if (_config.Debug)
            {
                _logger?.LogDebug("POST request to {Url}", url);
                if (data != null)
                {
                    _logger?.LogDebug("Request body: {Body}", JsonSerializer.Serialize(data, _jsonOptions));
                }
            }

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            if (_config.Debug)
            {
                _logger?.LogDebug("Response status code: {StatusCode}", response.StatusCode);
            }
            
            await HandleResponseAsync(response);
            
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }, cancellationToken);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data,
        Dictionary<string, string>? parameters = null, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, parameters);
        return await ExecuteWithRetryAsync(async () =>
        {
            using var request = CreateRequest(HttpMethod.Put, url, data);
            
            
            if (_config.Debug)
            {
                _logger?.LogDebug("PUT request to {Url}", url);
                _logger?.LogDebug("Request body: {Body}", JsonSerializer.Serialize(data, _jsonOptions));
            }
            
            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (_config.Debug)
            {
                _logger?.LogDebug("Response status code: {StatusCode}", response.StatusCode);
            }
            
            await HandleResponseAsync(response);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }, cancellationToken);
    }

    public async Task DeleteAsync(string endpoint, Dictionary<string, string>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(endpoint, parameters);
        await ExecuteWithRetryAsync(async () =>
        {
            using var request = CreateRequest(HttpMethod.Delete, url);

            if (_config.Debug)
            {
                _logger?.LogDebug("DELETE request to {Url}", url);
            }
            
            using var response = await _httpClient.SendAsync(request, cancellationToken);

            if (_config.Debug)
            {
                _logger?.LogDebug("Response status code: {StatusCode}", response.StatusCode);
            }
            
            await HandleResponseAsync(response);
            return Task.CompletedTask;
        }, cancellationToken);
    }

    private async Task<HttpResponseMessage> SendWithRedirectAsync(HttpRequestMessage request,
        CancellationToken cancellationToken, int maxRedirects = 10)
    {
        var currentRequest = request;
        HttpResponseMessage response = null!;

        for (int i = 0; i <= maxRedirects; i++)
        {
            response = await _httpClient.SendAsync(currentRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            var statusCode = (int)response.StatusCode;
            if (statusCode is 301 or 302 or 303 or 307 or 308)
            {
                var location = response.Headers.Location;
                if (location == null) break;

                if (_config.Debug)
                {
                    _logger?.LogDebug("Redirect {StatusCode} to {Location}", statusCode, location);
                }

                // Resolve relative redirect URLs against the base address
                var redirectUrl = location.IsAbsoluteUri
                    ? location.ToString()
                    : new Uri(_httpClient.BaseAddress!, location).ToString();

                // For 303 always use GET; for 301/302 preserve method for non-GET (debatable, but safe)
                var redirectMethod = statusCode == 303 ? HttpMethod.Get : currentRequest.Method;

                // Build a new request preserving the Authorization header
                var redirectRequest = new HttpRequestMessage(redirectMethod, redirectUrl);
                redirectRequest.Headers.Authorization = currentRequest.Headers.Authorization;
                redirectRequest.Headers.TryAddWithoutValidation("Accept", "application/json;charset=utf-8");
                redirectRequest.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                redirectRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("MoySklad-DotNet-Client", "1.0"));

                response.Dispose();
                currentRequest = redirectRequest;
            }
            else
            {
                break;
            }
        }

        return response;
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string url, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.Token);
        // Accept headers value must be exact: application/json;charset=utf-8 - no space, no uppercase
        request.Headers.TryAddWithoutValidation("Accept", "application/json;charset=utf-8");
        request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        request.Headers.Host = _httpClient.BaseAddress!.Host;
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("MoySklad-DotNet-Client", "1.0"));

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        return request;
    }


    private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        for (int attempt = 0; attempt < _config.RetryCount; attempt++)
        {
            try
            {
                return await action();
            }
            catch (RateLimitException ex) when (attempt < _config.RetryCount - 1)
            {
                var delay = ex.RetryAfter ?? _config.RetryDelay;
                _logger?.LogWarning(
                    "Rate limit exceeded. Waiting {Delay} seconds before retry (attempt {Attempt}/{Total}).",
                    delay.TotalSeconds, attempt + 1, _config.RetryCount);
                await Task.Delay(delay, cancellationToken);
            }
            catch (HttpRequestException ex) when (attempt < _config.RetryCount - 1)
            {
                var delay = TimeSpan.FromSeconds(_config.RetryDelay.TotalSeconds * Math.Pow(2, attempt));
                _logger?.LogWarning(ex,
                    "Connection error. Retrying in {Delay} seconds (attempt {Attempt}/{Total}).",
                    delay.TotalSeconds, attempt + 1, _config.RetryCount);
                await Task.Delay(delay, cancellationToken);
            }
        }

        throw new MoySkladException("Maximum retry attempts exceeded.");
    }

    private async Task HandleResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (_config.Debug)
            {
                _logger?.LogDebug("Response status: {StatusCode}", response.StatusCode);
            }

            return;
        }

        var content = await response.Content.ReadAsStringAsync();

        if (_config.Debug)
        {
            _logger?.LogDebug("Error response: {Content}", content);
        }

        var errorMessage = "Unknown Error";
        List<ValidationError>? validationErrors = null;

        try
        {
            var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(content, _jsonOptions);

            if (errorResponse?.Errors?.Any() == true)
            {
                errorMessage = string.Join("; ", errorResponse.Errors.Select(e => e.Error));
                validationErrors = errorResponse.Errors;
            }
            else if (!string.IsNullOrEmpty(errorResponse?.Error))
            {
                errorMessage = errorResponse.Error;
            }
        }
        catch
        {
            errorMessage = content;
        }

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized =>
                new AuthenticationException(errorMessage, content),

            HttpStatusCode.NotFound =>
                new NotFoundException(errorMessage, content),

            HttpStatusCode.TooManyRequests =>
                new RateLimitException(errorMessage, content, response.Headers.RetryAfter?.Delta),

            HttpStatusCode.BadRequest when validationErrors != null =>
                new ValidationException(errorMessage, validationErrors),

            HttpStatusCode.BadRequest => new ValidationException(errorMessage, content),

            _ when (int)response.StatusCode >= 400 && (int)response.StatusCode < 500 =>
                new ValidationException((int)response.StatusCode, errorMessage, content),

            _ => new MoySkladException((int)response.StatusCode, errorMessage, content)
        };
    }

    private static string BuildUrl(string endpoint, Dictionary<string, string>? parameters)
    {
        var url = endpoint.TrimStart('/');
        if (parameters?.Any() == true)
        {
            var query = string.Join("&", parameters.Select(kvp =>
                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            url += $"?{query}";
        }

        return url;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private record ApiErrorResponse
    {
        public string? Error { get; init; }
        public List<ValidationError>? Errors { get; init; }
    }
}