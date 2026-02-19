using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Base;

public abstract record BaseDocument : Entity
{
    // Ownership and sharing
    public Employee? Owner { get; init; }
    public bool? Shared { get; init; }
    public Group? Group { get; init; }
    
    // Basic document info
    public string? Description { get; init; }
    public string? ExternalCode { get; init; }
    
    [JsonPropertyName("moment")]
    public DateTime? Moment { get; init; }
    
    public bool? Applicable { get; init; }
    
    // Financial
    public CurrencyRate? Rate { get; init; }
    public decimal? Sum { get; init; }
    
    // Synchronization and state
    public string? SyncId { get; init; }
    public Project? Project { get; init; }
    public State? State { get; init; }
    
    // Additional data
    public List<EntityAttribute>? Attributes { get; init; }
    public DateTime? Deleted { get; init; }
    public Dictionary<string, object>? Files { get; init; }
    
    // Document Flags
    public bool? Printed { get; init; }
    public bool? Published { get; init; }
}

public abstract record BaseTradeDocument : BaseDocument
{
    public Counterparty? Agent { get; init; }
    public Organization? Organization { get; init; }
    
    // Bank accounts
    public OrganizationAccount? OrganizationAccount { get; init; }
    public OrganizationAccount? AgentAccount { get; init; }
    
    // Store
    public Store? Store { get; init; }
    
    // Positions (line items)
    public ListEntity<Position>? Positions { get; init; }
    
    // Tax information
    public string? TaxSystem { get; init; }
    public bool? VatEnabled { get; init; }
    public bool? VatIncluded { get; init; }
    public decimal? VatSum { get; init; }
}

public record State : Entity
{
    public int? Color { get; init; }
    public string? EntityType { get; init; }
    public string? StateType { get; init; }
}