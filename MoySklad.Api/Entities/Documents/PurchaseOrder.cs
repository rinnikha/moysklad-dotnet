using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record PurchaseOrder : BaseTradeDocument
{
    public string? ShipmentAddress { get; init; }
    public DateTime? DeliveryPlannedMoment { get; init; }
    
    public decimal? ShippedSum { get; init; }
    public decimal? PayedSum { get; init; }
    public decimal? InvoicedSum { get; init; }
    public decimal? ReservedSum { get; init; }
    
    public decimal? WaitSum { get; init; }
    
    // Related documents
    public List<CustomerOrder>? CustomerOrders { get; init; }
    public List<Supply>? Supplies { get; init; }
    public List<BaseDocument>? Payments { get; init; }
    public List<InvoiceIn>? InvoicesOut { get; init; }
}