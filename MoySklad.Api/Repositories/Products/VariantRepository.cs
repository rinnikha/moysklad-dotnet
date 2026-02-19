using MoySklad.Api.Client;
using MoySklad.Api.Entities.Products;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Products;

public class VariantRepository : EntityRepository<Variant>
{
    public VariantRepository(ApiClient apiClient)
        : base(apiClient, "entity/variant")
    {
    }

    /// <summary>
    /// Get all variants for a specific product.
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of variants</returns>
    public async Task<List<Variant>> GetByProductAsync(
        string productId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product ID cannot be null or empty", nameof(productId));

        var query = Query().Eq("product.id", productId);
        var response = await FindAllAsync(query, cancellationToken);

        return response?.Rows ?? new List<Variant>();
    }
}