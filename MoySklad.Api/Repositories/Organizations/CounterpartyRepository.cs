using MoySklad.Api.Client;
using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Organizations;

public class CounterpartyRepository : EntityRepository<Counterparty>
{
    public CounterpartyRepository(ApiClient apiClient)
        : base(apiClient, "entity/counterparty")
    {
    }

    public async Task<List<ContactPerson>> GetContactPersonsAsync(
        string counterpartyId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(counterpartyId))
            throw new ArgumentException("Counterparty ID cannot be null or empty", nameof(counterpartyId));

        var response = await ApiClient.GetAsync<Dictionary<string, object>>(
            $"{EntityName}/{counterpartyId}/contactpersons",
            cancellationToken: cancellationToken);

        // Parse response and convert to ContactPerson list
        // Implementation depends on your ContactPerson entity structure
        return new List<ContactPerson>(); // TODO: Parse response
    }

    public async Task<ContactPerson?> AddContactPersonAsync(
        string counterpartyId,
        ContactPerson contactPerson,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(counterpartyId))
            throw new ArgumentException("Counterparty ID cannot be null or empty", nameof(counterpartyId));

        if (contactPerson == null)
            throw new ArgumentNullException(nameof(contactPerson));

        var response = await ApiClient.PostAsync<List<ContactPerson>>(
            $"{EntityName}/{counterpartyId}/contactpersons",
            contactPerson,
            cancellationToken: cancellationToken);

        return response?.FirstOrDefault();
    }

    public async Task<List<Counterparty>> FindByPhoneAsync(
        string phone,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone cannot be null or empty", nameof(phone));

        var query = Query().Eq("phone", phone);
        var response = await FindAllAsync(query, cancellationToken);

        return response?.Rows ?? new List<Counterparty>() ;
    }

    public async Task<List<Counterparty>> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty", nameof(email));

        var query = Query().Eq("email", email);
        var response = await FindAllAsync(query, cancellationToken);

        return response?.Rows ?? new List<Counterparty>();
    }

    public async Task<List<Counterparty>> FindByInnAsync(
        string inn,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(inn))
            throw new ArgumentException("INN cannot be null or empty", nameof(inn));

        var query = Query().Eq("inn", inn);
        var response = await FindAllAsync(query, cancellationToken);

        return response?.Rows ?? new List<Counterparty>();
    }
}