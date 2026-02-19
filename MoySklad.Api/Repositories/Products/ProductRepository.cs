using MoySklad.Api.Client;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Products;
using MoySklad.Api.Query;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Products;

public class ProductRepository : EntityRepository<Product>
{
    public ProductRepository(ApiClient apiClient)
        : base(apiClient, "entity/product")
    {
    }

    public async Task<Dictionary<string, object>?> GetStockAsync(
        string productId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty", nameof(productId));

        return await ApiClient.GetAsync<Dictionary<string, object>>(
            $"{EntityName}/{productId}/stock",
            cancellationToken: cancellationToken);
    }
    
    public async Task<Product?> FindByBarcodeAsync(
        string barcode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            throw new ArgumentException("Barcode cannot be null or empty", nameof(barcode));

        var query = Query().Eq("barcodes", barcode).Limit(1);
        var response = await FindAllAsync(query, cancellationToken);

        return response.Rows?.FirstOrDefault();
    }
    
    public async Task<ListEntity<Product>> GetByFolderAsync(
        string folderId,
        QueryBuilder? query = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(folderId))
            throw new ArgumentException("Folder ID cannot be null or empty", nameof(folderId));

        var queryBuilder = query ?? Query();
        queryBuilder.Eq("productFolder.id", folderId);

        return await FindAllAsync(queryBuilder, cancellationToken);
    }
    
    public async Task<Product?> UpdatePricesAsync(
        string productId,
        List<Price> prices,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty", nameof(productId));

        if (prices == null || !prices.Any())
            throw new ArgumentException("Prices list cannot be null or empty", nameof(prices));

        var data = new { salePrices = prices };
        return await ApiClient.PutAsync<Product>(
            $"{EntityName}/{productId}",
            data,
            cancellationToken: cancellationToken);
    }
}

public class AssortmentRepository : EntityRepository<Assortment>
{
    public AssortmentRepository(ApiClient apiClient)
        : base(apiClient, "entity/assortment")
    {
    }

    public async Task<Assortment?> FindByExternalCode(string externalCode,
        CancellationToken cancellationToken = default)
    {
        if (String.IsNullOrWhiteSpace(externalCode))
        {
            throw new ArgumentException("ExternalCode cannot be null or empty", nameof(externalCode));
        }

        var query = Query().Eq("externalCode", externalCode);
        var response = await FindAllAsync(query, cancellationToken);
        
        return response.Rows?.FirstOrDefault();
    }
}