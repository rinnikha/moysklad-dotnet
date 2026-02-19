namespace MoySklad.Api.Query;

public class QueryBuilder
{
    private readonly List<string> _filters = new();
    private readonly List<string> _orderBy = new();
    private readonly List<string> _expand = new();
    private int? _limit;
    private int? _offset;
    private string? _search;

    public QueryBuilder Filter(string field, string op, object value)
    {
        _filters.Add($"{field}{op}{value}");
        return this;
    }

    public QueryBuilder Eq(string field, object value) => Filter(field, "=", value);
    public QueryBuilder Neq(string field, object value) => Filter(field, "!=", value);
    public QueryBuilder Gt(string field, object value) => Filter(field, ">", value);
    public QueryBuilder Gte(string field, object value) => Filter(field, ">=", value);
    public QueryBuilder Lt(string field, object value) => Filter(field, "<", value);
    public QueryBuilder Lte(string field, object value) => Filter(field, "<=", value);

    public QueryBuilder OrderBy(string field, string direction = "asc")
    {
        _orderBy.Add($"{field},{direction}");
        return this;
    }

    public QueryBuilder Limit(int limit)
    {
        _limit = limit;
        return this;
    }

    public QueryBuilder Offset(int offset)
    {
        _offset = offset;
        return this;
    }

    public QueryBuilder Expand(params string[] fields)
    {
        _expand.AddRange(fields);
        return this;
    }

    public QueryBuilder Search(string query)
    {
        _search = query;
        return this;
    }

    public Dictionary<string, string> ToParameters()
    {
        var parameters = new Dictionary<string, string>();

        if (_filters.Any())
            parameters["filter"] = string.Join(";", _filters);

        if (_orderBy.Any())
            parameters["order"] = string.Join(";", _orderBy);
        
        if (_expand.Any())
            parameters["expand"] = string.Join(",", _expand);

        if (_limit.HasValue)
            parameters["limit"] = _limit.Value.ToString();

        if (_offset.HasValue)
            parameters["offset"] = _offset.Value.ToString();

        if (!string.IsNullOrEmpty(_search))
            parameters["search"] = _search;
        
        return parameters;
    }
}