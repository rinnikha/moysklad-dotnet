using System.Text.Json.Serialization;

namespace MoySklad.Api.Entities.Base;

public abstract record Entity
{
    public Meta? Meta { get; init; }
    public string? Id { get; init; }
    public string? AccountId { get; init; }
    public string? Name { get; init; }
    
    [JsonPropertyName("created")]
    public DateTime? Created { get; init; }
    
    [JsonPropertyName("updated")]
    public DateTime? Updated { get; init; }

    public string? GetIdFromMetaHref()
    {
        if (Meta?.Href == null)
            return null;

        var segments = Meta.Href.Split('/');
        return segments.Length > 0 ? segments[^1] : null;
    }
}