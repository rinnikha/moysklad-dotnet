using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record Demand : BaseTradeDocument
{
    public string? ShipmentAddress { get; init; }
    public ShipmentAddress? ShipmentAddressFull { get; init; }
    
    public decimal? PayedSum { get; init; }
    
    public CustomerOrder? CustomerOrder { get; init; }
    public List<InvoiceOut>? InvoicesOut { get; init; }
}