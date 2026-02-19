namespace MoySklad.Api.Client;

public record MoySkladConfig
{
    public required string Token { get; init; }
    public string BaseUrl { get; set; } = "https://api.moysklad.ru/api/remap/1.2/";
    public int RetryCount { get; set; } = 3;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(1);
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(60);
    public bool Debug { get; init; }
    
    // This Api client User's timezone for converting dates to/from Moscow time.
    // If not set, dates are returned in UTC.
    // MoySklad API always uses Moscow time (UTC+3).
    public TimeZoneInfo? UserTimeZone { get; init; } = TimeZoneInfo.Utc;

    public MoySkladConfig WithTimeZone(string timeZoneId)
    {
        return this with { UserTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId) };
    }
}