using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class MoveRepository : TradeDocumentRepository<Move>
{
    public MoveRepository(ApiClient apiClient)
        : base(apiClient, "entity/move")
    {
    }
}