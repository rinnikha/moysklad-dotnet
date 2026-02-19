using Microsoft.Extensions.Logging;
using MoySklad.Api.Repositories.Documents;
using MoySklad.Api.Repositories.Organizations;
using MoySklad.Api.Repositories.Products;

namespace MoySklad.Api.Client;

public class MoySkladClient : IDisposable
{
    private readonly ApiClient _apiClient;

    // Products
    public AssortmentRepository Assortments { get; }
    public ProductRepository Products { get; }
    public ProductFolderRepository ProductFolders { get; }
    public CurrencyRepository Currencies { get; }

    // Organizations
    public OrganizationRepository Organizations { get; }
    public CounterpartyRepository Counterparties { get; }
    public EmployeeRepository Employees { get; }

    // Documents
    public CustomerOrderRepository CustomerOrders { get; }
    public DemandRepository Demands { get; }
    public SupplyRepository Supplies { get; }
    public InvoiceOutRepository InvoicesOut { get; }
    public InvoiceInRepository InvoicesIn { get; }
    public PurchaseOrderRepository PurchaseOrders { get; }

    public MoySkladClient(string token, bool debug = false, TimeZoneInfo? userTimeZone = null,
        ILogger<ApiClient>? logger = null)
        : this(new MoySkladConfig { Token = token, Debug = debug, UserTimeZone = userTimeZone }, logger)
    {
    }

    public MoySkladClient(MoySkladConfig config, ILogger<ApiClient>? logger = null)
    {
        _apiClient = new ApiClient(config, logger);

        // Products
        Assortments = new AssortmentRepository(_apiClient);
        Products = new ProductRepository(_apiClient);
        ProductFolders = new ProductFolderRepository(_apiClient);
        Currencies = new CurrencyRepository(_apiClient);

        // Organizations
        Organizations = new OrganizationRepository(_apiClient);
        Counterparties = new CounterpartyRepository(_apiClient);
        Employees = new EmployeeRepository(_apiClient);

        // Documents
        CustomerOrders = new CustomerOrderRepository(_apiClient);
        Demands = new DemandRepository(_apiClient);
        Supplies = new SupplyRepository(_apiClient);
        InvoicesOut = new InvoiceOutRepository(_apiClient);
        InvoicesIn = new InvoiceInRepository(_apiClient);
        PurchaseOrders = new PurchaseOrderRepository(_apiClient);
    }

    public async Task<Dictionary<string, object>?> SearchAsync(string query,
        CancellationToken cancellationToken = default)
    {
        return await _apiClient.GetAsync<Dictionary<string, object>>("entity/search",
            new Dictionary<string, string> { ["search"] = query }, cancellationToken);
    }

    public void Dispose()
    {
        _apiClient?.Dispose();
    }
}