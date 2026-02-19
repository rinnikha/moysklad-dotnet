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

public class InvoiceOutRepository : TradeDocumentRepository<InvoiceOut>
{
    public InvoiceOutRepository(ApiClient apiClient)
        : base(apiClient, "entity/invoiceout")
    {
    }
}

public class InvoiceInRepository : TradeDocumentRepository<InvoiceIn>
{
    public InvoiceInRepository(ApiClient apiClient)
        : base(apiClient, "entity/invoicein")
    {
    }
}

public class PurchaseOrderRepository : TradeDocumentRepository<PurchaseOrder>
{
    public PurchaseOrderRepository(ApiClient apiClient)
        : base(apiClient, "entity/purchaseorder")
    {
    }
}