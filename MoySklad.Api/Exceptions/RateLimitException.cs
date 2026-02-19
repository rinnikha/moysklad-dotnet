namespace MoySklad.Api.Exceptions;

public class RateLimitException : MoySkladException
{
    public TimeSpan? RetryAfter { get; }
    
    public RateLimitException(string message, TimeSpan? retryAfter = null)
    : base(429, message)
    {
        RetryAfter = retryAfter ?? TimeSpan.FromSeconds(60);
    }

    public RateLimitException(string message, string? responseBody, TimeSpan? retryAfter = null)
        : base(429, message, responseBody)
    {
        RetryAfter = retryAfter ?? TimeSpan.FromSeconds(60);
    }

    public RateLimitException(string message, int retryAfterSeconds)
        : base(429, message)
    {
        RetryAfter = TimeSpan.FromSeconds(retryAfterSeconds);
    }

    public RateLimitException(string message, Exception innerException, TimeSpan? retryAfter = null)
        : base(429, message, innerException)
    {
        RetryAfter = retryAfter ?? TimeSpan.FromSeconds(60);
    }
}