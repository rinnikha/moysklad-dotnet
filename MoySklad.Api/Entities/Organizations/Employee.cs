using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Organizations;

public record Employee : Entity
{
    public bool? Archived { get; init; }
    
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }
    public string? MiddleName { get; init; }
    public string? ShortFio { get; init; }
    public string? Uid { get; init; }
    public string? Email {get; init;}
    public string? Phone {get; init;}
    public string? Position {get; init;}
    public string? Description { get; init; }
    public string? ExternalCode { get; init; }
    public string? Code { get; init; }
    public Employee? Owner { get; init; }
    public bool? Shared { get; init; }
    public Group? Group { get; init; }
    public List<EntityAttribute>? Attributes { get; init; }
    public List<Cashier>? Cashiers { get; init; }
    public Image? Image { get; init; }
    public Dictionary<string, object>? Salary { get; init; }
}

public record Image
{
    [JsonPropertyName("filename")]
    public string? FileName { get; init; }
    public Meta? Meta { get; init; }
    public int? Size { get; init; }
    public Meta? Tiny { get; init; }
    public string? Title { get; init; }
    public DateTime Updated { get; init; }
    
    // Image in Base64 (for PUT request)
    public string? Content { get; init; }
}

public record Cashier
{
    public string? AccountId { get; init; }
    public Employee? Employee { get; init; }
    public string? Id { get; init; }
    public Entity? RetailStore { get; init; }
}

public record Group : Entity
{
    public int? Index { get; init; }
}