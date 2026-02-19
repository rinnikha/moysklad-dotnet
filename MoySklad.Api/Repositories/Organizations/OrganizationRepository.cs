using MoySklad.Api.Client;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Organizations;

public class OrganizationRepository : EntityRepository<Organization>
{
    public OrganizationRepository(ApiClient apiClient)
        : base(apiClient, "entity/organization")
    {
    }
}