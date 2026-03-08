using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class EnterRepository : TradeDocumentRepository<Enter>
{
    public EnterRepository(ApiClient apiClient)
        : base(apiClient, "entity/enter")
    
    {
    }
}