using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record InvoiceOut : BaseTradeDocument
{
    public DateTime? PaymentPlannedMoment { get; init; }
    public decimal? PayedSum { get; init; }
    
    public List<BaseDocument>? Payments { get; init; }
    public List<Demand>? Demands { get; init; }
    public List<CustomerOrder>? CustomerOrders { get; init; }
    
    public Contract? Contract { get; init; }
    public Dictionary<string, object>? SalesChannel { get; init; }
    public decimal? ShippedSum { get; init; }
}