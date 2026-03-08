using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Documents;

public record Move : BaseTradeDocument
{
    public Dictionary<string, object>? Overhead { get; init; }
    
    public Store? SourceStore { get; init; }
    public Store? TargetStore { get; init; }
    
    // Related
    public Demand? Demand { get; init; }
    public Dictionary<string, object>? InternalOrder { get; init; }
    public CustomerOrder? CustomerOrder { get; init; }
    public Supply? Supply { get; init; }
}