using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record InvoiceIn : BaseTradeDocument
{
    public DateTime? PaymentPlannedMoment { get; init; }
    public decimal? PayedSum { get; init; }
    
    public List<BaseDocument>? Payments { get; init; }
    public List<Supply>? Supplies { get; init; }
    public List<CustomerOrder>? PurchaseOrder { get; init; }
    
    public Contract? Contract { get; init; }
    public decimal? ShippedSum { get; init; } 
}