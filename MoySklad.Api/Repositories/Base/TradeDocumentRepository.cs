using MoySklad.Api.Client;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Query;

namespace MoySklad.Api.Repositories.Base;

/// <summary>
/// Base repository for trade documents that have a Positions sub-resource
/// (CustomerOrder, Demand, Supply, InvoiceOut, InvoiceIn, PurchaseOrder, etc.).
/// </summary>
public abstract class TradeDocumentRepository<T> : EntityRepository<T> where T : BaseTradeDocument
{
    protected TradeDocumentRepository(ApiClient apiClient, string entityName)
        : base(apiClient, entityName)
    {
    }

    /// <summary>
    /// Loads the positions for a document by its ID.
    /// Returns the first page (up to 1000 items) along with the metadata.
    /// </summary>
    public async Task<ListEntity<Position>> GetPositionsAsync(
        string documentId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        var response = await ApiClient.GetAsync<ListEntity<Position>>(
            $"{EntityName}/{documentId}/positions",
            cancellationToken: cancellationToken);



        return response ?? new ListEntity<Position> { Rows = new List<Position>(), Meta = Meta.CreateDefault() };
    }

    /// <summary>
    /// Loads ALL positions for a document by its ID, automatically paginating through all pages.
    /// Use this when a document has more than 1000 positions.
    /// </summary>
    public async Task<List<Position>> FetchAllPositionsAsync(
        string documentId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        var allPositions = new List<Position>();
        const int pageSize = 1000;
        var offset = 0;

        while (true)
        {
            var parameters = new Dictionary<string, string>
            {
                ["limit"] = pageSize.ToString(),
                ["offset"] = offset.ToString()
            };

            var response = await ApiClient.GetAsync<ListEntity<Position>>(
                $"{EntityName}/{documentId}/positions",
                parameters,
                cancellationToken);

            if (response?.Rows == null || !response.Rows.Any())
                break;

            allPositions.AddRange(response.Rows);

            var totalSize = response.Meta?.Size ?? 0;
            offset += response.Rows.Count;

            if (offset >= totalSize)
                break;
        }

        return allPositions;
    }

    /// <summary>
    /// Loads positions directly from the Positions stub on a document.
    /// Reads the href from document.Positions.Meta.Href and makes the API request.
    /// Automatically fetches ALL pages if there are more than 1000 positions.
    /// </summary>
    /// <example>
    /// var order = await client.CustomerOrders.FindByIdAsync(id);
    /// var positions = await client.CustomerOrders.LoadPositionsAsync(order);
    /// </example>
    public async Task<List<Position>> LoadPositionsAsync(
        T document,
        CancellationToken cancellationToken = default)
    {
        if (document == null)
            throw new ArgumentNullException(nameof(document));

        // If positions are already fully loaded (Rows is not null), return them directly
        if (document.Positions?.Rows != null)
            return document.Positions.Rows;

        // Use the href from Meta if available, otherwise fall back to entity ID
        var href = document.Positions?.Meta?.Href;
        if (!string.IsNullOrEmpty(href))
        {
            return await FetchAllPositionsByHrefAsync(href, cancellationToken);
        }

        // Fall back to ID-based fetch
        if (string.IsNullOrEmpty(document.Id))
            throw new InvalidOperationException(
                "Cannot load positions: document has neither a Positions.Meta.Href nor an Id.");

        return await FetchAllPositionsAsync(document.Id, cancellationToken);
    }

    private async Task<List<Position>> FetchAllPositionsByHrefAsync(
        string href,
        CancellationToken cancellationToken)
    {
        var allPositions = new List<Position>();
        const int pageSize = 1000;
        var offset = 0;

        // Parse the base href without any existing query params
        var baseHref = href.Contains('?') ? href.Substring(0, href.IndexOf('?')) : href;

        while (true)
        {
            var pagedHref = $"{baseHref}?limit={pageSize}&offset={offset}";
            var response = await ApiClient.GetByHrefAsync<ListEntity<Position>>(pagedHref, cancellationToken);

            if (response?.Rows == null || !response.Rows.Any())
                break;

            allPositions.AddRange(response.Rows);

            var totalSize = response.Meta?.Size ?? 0;
            offset += response.Rows.Count;

            if (offset >= totalSize)
                break;
        }

        return allPositions;
    }

    /// <summary>
    /// Creates a new position on the document.
    /// </summary>
    public async Task<Position?> CreatePositionAsync(
        string documentId,
        Position position,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        if (position == null)
            throw new ArgumentNullException(nameof(position));

        var response = await ApiClient.PostAsync<List<Position>>(
            $"{EntityName}/{documentId}/positions",
            position,
            cancellationToken: cancellationToken);

        return response?.FirstOrDefault();
    }

    /// <summary>
    /// Updates an existing position on the document.
    /// </summary>
    public async Task<Position?> UpdatePositionAsync(
        string documentId,
        string positionId,
        Position position,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        if (string.IsNullOrWhiteSpace(positionId))
            throw new ArgumentException("Position ID cannot be null or empty", nameof(positionId));

        if (position == null)
            throw new ArgumentNullException(nameof(position));

        return await ApiClient.PutAsync<Position>(
            $"{EntityName}/{documentId}/positions/{positionId}",
            position,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Deletes a position from the document.
    /// </summary>
    public async Task DeletePositionAsync(
        string documentId,
        string positionId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentId))
            throw new ArgumentException("Document ID cannot be null or empty", nameof(documentId));

        if (string.IsNullOrWhiteSpace(positionId))
            throw new ArgumentException("Position ID cannot be null or empty", nameof(positionId));

        await ApiClient.DeleteAsync(
            $"{EntityName}/{documentId}/positions/{positionId}",
            cancellationToken: cancellationToken);
    }
}