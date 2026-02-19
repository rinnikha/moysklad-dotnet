namespace MoySklad.Api.Entities.Base;

public record Meta
{
    public required string Href { get; init; }
    public string? MetadataHref { get; init; }
    public string? Type { get; init; }
    public string? MediaType { get; init; }
    public string? UuidHref { get; init; }
    public int? Size { get; init; }
    public int? Limit { get; init; }
    public int? Offset { get; init; }

    public static Meta CreateDefault(string href = "") => new() { Href = href };
}