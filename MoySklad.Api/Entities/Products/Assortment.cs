using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Entities.Products;

public record Assortment : BasePhysicalAssortment
{
    public double? Stock { get; init; }
    public double? Reserve { get; init; }
    public double? InTransit { get; init; }
    public double? Quantity { get; init; } 
    
    public List<Dictionary<string, object>>? Characteristics { get; init; }
}