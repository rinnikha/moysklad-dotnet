using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Base;

public abstract record BaseAssortment : Entity
{
    // Identification
    public string? Code { get; init; }
    public string? ExternalCode { get; init; }
    public string? Description { get; init; }
    public string? Article { get; set; }
    
    public int? Vat { get; init; }
    public int? EffectiveVat { get; init; }
    public bool? VatEnabled { get; init; }
    public bool? EffectiveVatEnabled { get; init; }
    public bool? UseParentVat { get; init; }
    public string? TaxSystem { get; init; }
    
    // Pricing
    [JsonPropertyName("salePrices")]
    public List<Price>? SalePrices { get; init; }
    
    [JsonPropertyName("buyPrice")]
    public Price? BuyPrice { get; init; }
    
    [JsonPropertyName("minPrice")]
    public Price? MinPrice { get; init; }
    
    public bool? DiscountProhibited { get; init; }
    
    // Organization
    public Dictionary<string, object>? ProductFolder { get; init; }
    public Dictionary<string, object>? Uom { get; init; }
    public string? PathName { get; init; }
    
    // Tracking and identification
    public List<Dictionary<string, object>>? Barcodes { get; init; }
    public string? PaymentItemType { get; init; }
    
    // Media
    public Dictionary<string, object>? Images { get; init; }
    public Dictionary<string, object>? Files { get; init; }
    
    public Counterparty? Supplier { get; init; }
    
    // Sharing
    public Dictionary<string, object>? Owner { get; init; }
    public bool? Shared { get; init; }
    public Dictionary<string, object>? Group { get; init; }
    public bool? Archived { get; init; }
    
    // Custom fields
    public List<EntityAttribute>? Attributes { get; init; }
    
    // Sync
    public string? SyncId { get; init; }
}

public abstract record BasePhysicalAssortment : BaseAssortment
{
    public bool? Weighed { get; init; }
    public decimal? Weight { get; init; }
    public decimal? Volume { get; init; }
    
    public int? VariantsCount { get; init; }
 
    // Tracking
    public bool? IsSerialTrackable { get; init; }
    public string? TrackingType { get; init; }
    
    // Country of origin
    public Dictionary<string, object>? Country { get; init; }
    
    // Packs
    public List<Dictionary<string, object>>? Packs { get; init; }
}
