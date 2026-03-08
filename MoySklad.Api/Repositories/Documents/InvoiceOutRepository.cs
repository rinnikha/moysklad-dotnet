using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class InvoiceOutRepository : TradeDocumentRepository<InvoiceOut>
{
    public InvoiceOutRepository(ApiClient apiClient)
        : base(apiClient, "entity/invoiceout")
    {
    }
}