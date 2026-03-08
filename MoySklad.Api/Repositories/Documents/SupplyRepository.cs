using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class SupplyRepository : TradeDocumentRepository<Supply>
{
    public SupplyRepository(ApiClient apiClient)
        : base(apiClient, "entity/supply")
    {
    }
}