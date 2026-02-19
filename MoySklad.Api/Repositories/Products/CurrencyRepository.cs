using MoySklad.Api.Client;
using MoySklad.Api.Entities.Products;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Products;

public class CurrencyRepository : EntityRepository<Currency>
{
    public CurrencyRepository(ApiClient apiClient)
        : base(apiClient, "entity/currency")
    {
    }

    public async Task<Currency?> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        var query = Query()
            .Eq("default", "true")
            .Eq("archived", "false")
            .Limit(1);

        var response = await FindAllAsync(query, cancellationToken);
        return response.Rows?.FirstOrDefault();
    }
}