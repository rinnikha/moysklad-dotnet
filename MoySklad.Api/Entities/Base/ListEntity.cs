using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Base;

public record ListEntity<T>
{
    public Meta? Meta { get; init; }
    public List<T>? Rows { get; init; }
    public Context? Context { get; init; }
}

public record Context
{
    public Employee? Employee { get; init; }
}