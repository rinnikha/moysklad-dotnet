using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class LossRepository : TradeDocumentRepository<Loss>
{
    public LossRepository(ApiClient apiClient)
        : base(apiClient, "entity/loss")
    {
    }
}