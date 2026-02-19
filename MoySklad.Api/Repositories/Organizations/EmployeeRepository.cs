using MoySklad.Api.Client;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Organizations;

public class EmployeeRepository : EntityRepository<Employee>
{
    public EmployeeRepository(ApiClient apiClient)
        : base(apiClient, "entity/employee")
    {
    }

    public async Task<Employee?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        return await ApiClient.GetAsync<Employee>(
            "context/employee",
            cancellationToken: cancellationToken);
    }
}