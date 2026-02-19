using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Products;

public record Product : BasePhysicalAssortment
{
    public Counterparty? Supplier { get; init; }
    
    public int? VariantsCount { get; init; }
}

public record Service : BaseAssortment
{
}

public record Bundle : BasePhysicalAssortment
{
    public List<Dictionary<string, object>>? Components { get; init; }
    public Dictionary<string, object>? Overhead { get; init; }
    public bool? PartialDisposal { get; init; }
    public string? Tnved { get; init; }
}

public record Variant : BasePhysicalAssortment
{
    public Product? Product { get; init; }
    
    public List<Dictionary<string, object>>? Characteristics { get; init; }
}

public record ProductFolder : Entity
{
    public string? PathName { get; init; }
    public string? Code { get; init; }
    public string? ExternalCode { get; init; }
    public bool? Archived { get; init; }
    public int? Vat { get; init; }
    public int? EffectiveVat { get; init; }
    public bool? UseParentVat { get; init; }
    public string? TaxSystem { get; init; }
    public bool? Shared { get; init; }
    public Employee? Owner { get; init; }
    public Group? Group { get; init; }
    public ProductFolder? Product { get; init; }
}


public record Price
{
    public decimal? Value { get; init; }
    public Currency? Currency { get; init; }
    public PriceType? PriceType { get; init; }
}

public record PriceType
{
    public Meta? Meta { get; init; }
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? ExternalCode { get; init; }
}

public record Currency : Entity
{
    public bool? Default { get; init; }
    public string? FullName { get; init; }
    public bool? Indirect { get; init; }
    public string? IsoCode { get; init; }
    public Dictionary<string, string>? MajorUnit { get; init; }
    public Dictionary<string, string>? MinorUnit { get; init; }
    public double? Margin { get; init; }
    public int? Multiplicity { get; init; }
    public double? Rate { get; init; }
    public string? RateUpdateType { get; init; }
    public bool? System { get; init; }
}

public record CurrencyRate
{
    public Currency? Currency { get; init; }
    public decimal? Value { get; init; }
}



public record Project : Entity
{
    public bool? Archived { get; init; }
    public List<EntityAttribute>? Attributes { get; init; }
    public string? Code { get; init; }
    public string? Description { get; init; }
    public string? ExternalCode { get; init; }
    public Group? Group { get; init; }
    public Employee? Owner { get; init; }
    public bool? Shared { get; init; }
}

