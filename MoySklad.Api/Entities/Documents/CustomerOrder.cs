using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Documents;

public record CustomerOrder : BaseTradeDocument
{
    public string? ShipmentAddress { get; init; }
    public ShipmentAddress? ShipmentAddressFull { get; init; }
    public DateTime? DeliveryPlannedMoment { get; init; }
    
    public decimal? ShippedSum { get; init; }
    public decimal? PayedSum { get; init; }
    public decimal? InvoicedSum { get; init; }
    public decimal? ReservedSum { get; init; }
    
    public Entity? SalesChannel { get; init; }
    
    // Related documents
    public List<PurchaseOrder>? PurchaseOrders { get; init; }
    public List<Demand>? Demands { get; init; }
    public List<Dictionary<string, object>>? Payments { get; init; }
    public List<InvoiceOut>? InvoicesOut { get; init; }
    public List<Dictionary<string, object>>? Moves { get; init; }
    [JsonPropertyName("prepayments")]
    public List<BaseDocument>? PrePayments { get; init; }
}

public record Position : Entity
{
    public decimal? Quantity { get; init; }
    public decimal? Price { get; init; }
    public decimal? Discount { get; init; }
    public int? Vat { get; init; }
    public bool? VatEnabled { get; init; }
    public Assortment? Assortment { get; init; }
    public decimal? Shipped { get; init; }
    public decimal? Reserve { get; init; }
}

public record ShipmentAddress
{
    public string? AddInfo { get; init; }
    public string? Apartment { get; init; }
    public string? City { get; init; }
    public string? Comments { get; init; }
    public string? Country { get; init; }
    public string? House { get; init; }
    public string? PostalCode { get; init; }
    public Entity? Region { get; init; }
    public string? Street { get; init; }
}

public record Contract : Entity
{
    public Counterparty? Agent { get; init; }
    public OrganizationAccount? AgentAccount { get; init; }
    public bool? Archived { get; init; }
    public List<EntityAttribute>? Attributes { get; init; }
    public string? Code { get; init; }
    public string? ContractType { get; init; }
    public string? Description { get; init; }
    public string? ExternalCode { get; init; }
    public DateTime? Moment { get; init; }
    public Organization? OwnAgent { get;  init; }
    public Employee? Owner { get; init; }
    public CurrencyRate? Rate { get; init; }
    public int? RewardPercent { get; init; }
    public string? RewardType { get; init; }
    public bool? Shared { get; init; }
    public bool? Printed { get; init; }
    public bool? Published { get; init; }
}