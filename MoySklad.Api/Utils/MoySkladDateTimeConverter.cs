using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoySklad.Api.Utils;

public class MoySkladDateTimeConverter : JsonConverter<DateTime?>
{
    private static readonly TimeZoneInfo MoscowTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");

    private static readonly string MoySkladDateFormat = "yyyy-MM-dd HH:mm:ss.fff";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unexpected token type: {reader.TokenType}");
        }

        var dateString = reader.GetString();
        if (string.IsNullOrWhiteSpace(dateString))
        {
            return null;
        }

        if (DateTime.TryParseExact(dateString, MoySkladDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var parsedDate))
        {
            var moscowTime = DateTime.SpecifyKind(parsedDate, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(moscowTime, MoscowTimeZone);
        }

        throw new JsonException($"Unable to parse datetime: {dateString}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }
        
        // Convert UTC to Moscow time for sending to API
        var utcTime = value.Value.Kind == DateTimeKind.Utc
            ? value.Value
            : value.Value.ToUniversalTime();
        
        var moscowTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, MoscowTimeZone);
        
        writer.WriteStringValue(moscowTime.ToString(MoySkladDateFormat, CultureInfo.InvariantCulture));
    }
}