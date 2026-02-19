using MoySklad.Api.Client;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Query;

namespace MoySklad.Api.Repositories.Base;

public abstract class EntityRepository<T> where T : Entity
{
    protected readonly ApiClient ApiClient;
    protected readonly string EntityName;

    protected EntityRepository(ApiClient apiClient, string entityName)
    {
        ApiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
    }

    public virtual async Task<ListEntity<T>> FindAllAsync(
        QueryBuilder? query = null,
        CancellationToken cancellationToken = default)
    {
        var parameters = query?.ToParameters();
        var response = await ApiClient.GetAsync<ListEntity<T>>(EntityName, parameters, cancellationToken);

        return response ?? new ListEntity<T>
        {
            Meta = Meta.CreateDefault(),
            Rows = new List<T>(),
        };
    }

    public virtual async Task<List<T>> FetchAllAsync(
        QueryBuilder? query = null,
        CancellationToken cancellationToken = default)
    {
        var allItems = new List<T>();
        var pageSize = 1000;
        var offset = 0;

        var queryBuilder = query ?? Query();
        queryBuilder.Limit(pageSize);

        while (true)
        {
            queryBuilder.Offset(offset);
            
            var result = await FindAllAsync(queryBuilder, cancellationToken);

            if (result.Rows != null && result.Rows.Any())
            {
                allItems.AddRange(result.Rows);
            }

            var totalSize = result.Meta?.Size ?? 0;
            var fetchedCount = offset + (result.Rows?.Count ?? 0);

            if (fetchedCount >= totalSize || result.Rows == null || !result.Rows.Any())
            {
                break;
            }

            offset += pageSize;
        }

        return allItems;
    }

    public virtual async Task<T?> FindByIdAsync(string id, QueryBuilder? query = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Entity ID cannot be null or empty", nameof(id));

        var parameters = query?.ToParameters();
        return await ApiClient.GetAsync<T>($"{EntityName}/{id}", parameters, cancellationToken);
    }

    public virtual async Task<T?> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        return await ApiClient.PostAsync<T>(EntityName, entity, cancellationToken: cancellationToken);
    }

    public virtual async Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an ID for update", nameof(entity));

        return await ApiClient.PutAsync<T>($"{EntityName}/{entity.Id}", entity,
            cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Entity ID cannot be null or empty", nameof(id));

        await ApiClient.DeleteAsync($"{EntityName}/{id}", cancellationToken: cancellationToken);
    }

    public virtual async Task<List<T>> CreateBulkAsync(
        List<T> entities,
        CancellationToken cancellationToken = default)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities list cannot be null or empty", nameof(entities));

        var response = await ApiClient.PostAsync<ListEntity<T>>(
            EntityName,
            entities,
            cancellationToken: cancellationToken);

        return response?.Rows ?? new List<T>();
    }

    public virtual async Task<List<T>> UpdateBulkAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("Entities list cannot be null or empty", nameof(entities));

        if (entities.Any(e => string.IsNullOrEmpty(e.Id)))
            throw new ArgumentException("All entities must have an ID for bulk update");

        var response = await ApiClient.PostAsync<ListEntity<T>>(
            EntityName,
            entities,
            cancellationToken: cancellationToken);

        return response?.Rows ?? new List<T>();
    }

    public virtual async Task DeleteBulkAsync(
        List<string> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids == null || !ids.Any())
            throw new ArgumentException("IDs list cannot be null or empty", nameof(ids));

        await ApiClient.PostAsync<object>(
            $"{EntityName}/delete",
            ids,
            cancellationToken: cancellationToken);
    }

    public virtual async Task<Dictionary<string, object>?> GetMetadataAsync(
        CancellationToken cancellationToken = default)
    {
        return await ApiClient.GetAsync<Dictionary<string, object>>(
            $"{EntityName}/metadata", cancellationToken: cancellationToken);
    }

    public QueryBuilder Query() => new();
}