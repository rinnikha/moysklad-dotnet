namespace MoySklad.Api.Entities.Base;

public record EntityAttribute
{
    public string? Id { get; init; }
    public Meta? Meta { get; init; }
    public string? Name { get; init; }
    public string? Type { get; init; }
    public object? Value { get; init; }
    public Dictionary<string, object>? File { get; init; }

    public T? GetValue<T>()
    {
        if (Value == null) return default;

        if (Value is T typed) return typed;

        try
        {
            return Type switch
            {
                EntityAttributeType.Time => ConvertToDateTime<T>(Value),
                EntityAttributeType.Boolean => ConvertToBoolean<T>(Value),
                EntityAttributeType.Long => ConvertToLong<T>(Value),
                EntityAttributeType.Double => ConvertToDouble<T>(Value),
                EntityAttributeType.String or EntityAttributeType.Text or EntityAttributeType.Link =>
                    ConvertToString<T>(Value),
                EntityAttributeType.CustomEntity => ConvertToObject<T>(Value),
                _ => (T)Convert.ChangeType(Value, typeof(T))
            };
        }
        catch
        {
            return default;
        }
    }

    private static T? ConvertToDateTime<T>(object value)
    {
        if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
        {
            if (DateTime.TryParse(value.ToString(), out var dt))
                return (T)(object)dt;
        }
        if (typeof(T) == typeof(string))
            return (T)(object)value.ToString();
        return default;
    }
    
    private static T? ConvertToBoolean<T>(object value)
    {
        if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
        {
            if (value is bool b) return (T)(object)b;
            if (bool.TryParse(value.ToString(), out var result))
                return (T)(object)result;
        }
        if (typeof(T) == typeof(string))
            return (T)(object)value.ToString()!;
        return default;
    }
    
    private static T? ConvertToLong<T>(object value)
    {
        if (typeof(T) == typeof(long) || typeof(T) == typeof(long?))
        {
            if (value is long l) return (T)(object)l;
            if (long.TryParse(value.ToString(), out var result))
                return (T)(object)result;
        }
        if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
        {
            if (int.TryParse(value.ToString(), out var result))
                return (T)(object)result;
        }
        if (typeof(T) == typeof(string))
            return (T)(object)value.ToString()!;
        return default;
    }
    
    private static T? ConvertToDouble<T>(object value)
    {
        if (typeof(T) == typeof(decimal) || typeof(T) == typeof(decimal?))
        {
            if (value is decimal d) return (T)(object)d;
            if (decimal.TryParse(value.ToString(), out var result))
                return (T)(object)result;
        }
        if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
        {
            if (value is double db) return (T)(object)db;
            if (double.TryParse(value.ToString(), out var result))
                return (T)(object)result;
        }
        if (typeof(T) == typeof(string))
            return (T)(object)value.ToString()!;
        return default;
    }

    private static T? ConvertToString<T>(object value)
    {
        if (typeof(T) == typeof(string))
            return (T)(object)value.ToString()!;
        return default;
    }

    private static T? ConvertToObject<T>(object value)
    {
        if (value is T obj) return obj;
        if (typeof(T) == typeof(Dictionary<string, object>) && value is Dictionary<string, object> dict)
            return (T)(object)dict;
        return default;
    }

    /// <summary>
    /// Create a new attribute with updated value.
    /// </summary>
    public EntityAttribute WithValue(object? newValue) => this with { Value = newValue };
}

public static class EntityAttributeType
{
    public const string Time = "time";
    public const string CustomEntity = "customentity";
    public const string Link = "link";
    public const string String = "string";
    public const string Text = "text";
    public const string File = "file";
    public const string Boolean = "boolean";
    public const string Double = "double";
    public const string Long = "long";
}