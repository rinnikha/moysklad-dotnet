using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record Supply : BaseTradeDocument
{
    public decimal? PayedSum { get; init; }
    
    public string? IncomingNumber { get; init; }
    public DateTime? IncomingDate { get; init; }
    
    public List<InvoiceIn>? InvoicesIn { get; init; }
}