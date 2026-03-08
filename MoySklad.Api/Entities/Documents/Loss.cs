using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Documents;

public record Loss : BaseTradeDocument
{
    public Dictionary<string, object>? Overhead { get; init; }
    
    // Related
    public SalesReturn? SalesReturn { get; init; }
    public Entity? Inventory { get; init; }
}