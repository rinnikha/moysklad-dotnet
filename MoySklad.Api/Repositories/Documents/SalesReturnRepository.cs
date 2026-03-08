using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class SalesReturnRepository : TradeDocumentRepository<SalesReturn>
{
    public SalesReturnRepository(ApiClient apiClient)
        : base(apiClient, "entity/salesreturn")
    {
    }
}