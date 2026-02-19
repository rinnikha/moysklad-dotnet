using MoySklad.Api.Client;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Documents;
using MoySklad.Api.Query;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Documents;

public class CustomerOrderRepository : TradeDocumentRepository<CustomerOrder>
{
    public CustomerOrderRepository(ApiClient apiClient)
        : base(apiClient, "entity/customerorder")
    {
    }

    public async Task<ListEntity<CustomerOrder>> GetByAgentAsync(
        string agentId,
        QueryBuilder? query = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(agentId))
            throw new ArgumentException("Agent ID cannot be null or empty", nameof(agentId));

        var queryBuilder = query ?? Query();
        queryBuilder.Eq("agent.id", agentId);

        return await FindAllAsync(queryBuilder, cancellationToken);
    }
}

public class DemandRepository : TradeDocumentRepository<Demand>
{
    public DemandRepository(ApiClient apiClient)
        : base(apiClient, "entity/demand")
    {
    }
}