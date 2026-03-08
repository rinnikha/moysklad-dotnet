using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Documents;

public record SalesReturn : BaseTradeDocument
{
    public Contract? Contract { get; init; }
    
    // Related
    public Demand? Demand { get; init; }
    public Loss? Losses { get; init; }
    public List<Dictionary<string, object>>? Payments { get; init; }
    public decimal? PayedSum { get; init; }
    public Dictionary<string, object>? FactureOut { get; init; }
}