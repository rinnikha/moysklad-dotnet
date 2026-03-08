using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Documents;

public record Enter : BaseTradeDocument
{
    public Dictionary<string, object>? Overhead { get; init; }
    
    // Related
    public Entity? Inventory { get; init; }
}