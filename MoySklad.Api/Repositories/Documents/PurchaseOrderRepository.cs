using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class PurchaseOrderRepository : TradeDocumentRepository<PurchaseOrder>
{
    public PurchaseOrderRepository(ApiClient apiClient)
        : base(apiClient, "entity/purchaseorder")
    {
    }
}