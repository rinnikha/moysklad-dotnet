using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class DemandRepository : TradeDocumentRepository<Demand>
{
    public DemandRepository(ApiClient apiClient)
        : base(apiClient, "entity/demand")
    {
    }
}