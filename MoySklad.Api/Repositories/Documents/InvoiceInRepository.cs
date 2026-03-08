using MoySklad.Api.Client;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class InvoiceInRepository : TradeDocumentRepository<InvoiceIn>
{
    public InvoiceInRepository(ApiClient apiClient)
        : base(apiClient, "entity/invoicein")
    {
    }
}