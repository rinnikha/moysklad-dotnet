namespace MoySklad.Api.Utils;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo MoscowTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

    public static DateTime ToMoscowTime(this DateTime utcDateTime)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            utcDateTime = utcDateTime.ToUniversalTime();
        }

        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, MoscowTimeZone);
    }

    public static DateTime FromMoscowTime(this DateTime moscowDateTime)
    {
        var unspecified = DateTime.SpecifyKind(moscowDateTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, MoscowTimeZone);
    }

    public static DateTime ToUserTime(this DateTime utcDateTime, TimeZoneInfo userTimeZone)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            utcDateTime = utcDateTime.ToUniversalTime();
        }
        
        return TimeZoneInfo.ConvertTimeToUtc(utcDateTime, userTimeZone);
    }

    public static DateTime FromUserTime(this DateTime userDateTime, TimeZoneInfo userTimeZone)
    {
        var unspecified = DateTime.SpecifyKind(userDateTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, MoscowTimeZone);
    }
}