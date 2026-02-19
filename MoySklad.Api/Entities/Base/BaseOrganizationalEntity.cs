using System.Text.Json.Serialization;
using MoySklad.Api.Entities.Organizations;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Base;

public abstract record BaseOrganizationalEntity : Entity
{
    // Identification
    public string? Code { get; init; }
    public string? ExternalCode { get; init; }
    public string? Description { get; init; }
    
    // Company type and legal info
    public string? CompanyType { get; init; }
    public string? LegalTitle { get; init; }
    
    // Tax numbers
    public string? Inn { get; init; }
    public string? Kpp { get; init; }
    public string? Ogrn { get; init; }
    public string? Okpo { get; init; }
    
    // Addresses
    public string? LegalAddress { get; init; }
    public OrganizationAddress? LegalAddressFull { get; init; }
    public string? ActualAddress { get; init; }
    public OrganizationAddress? ActualAddressFull { get; init; }
    
    // Contact information
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    
    // Organization structure
    public Employee? Owner { get; init; }
    public bool? Shared { get; init; }
    public Group? Group { get; init; }
    public bool? Archived { get; init; }
    
    // Additional data
    public List<EntityAttribute>? Attributes { get; init; }
    public Dictionary<string, object>? Files { get; init; }
    public List<OrganizationAccount>? Accounts { get; init; }
}
public record OrganizationAddress
{
    public string? AddInfo { get; init; }
    public string? Apartment { get; init; }
    public string? City { get; init; }
    public string? Comment { get; init; }
    
    [JsonPropertyName("fiasCode_ru")]
    public string? FiasCodeRu { get; init; }
    public Entity? Country { get; init; }
    public string? House { get; init; }
    public string? PostalCode { get; init; }
    public Entity? Region { get; init; }
    public string? Street { get; init; }
}

public record OrganizationAccount : Entity
{
    public string? AccountNumber { get; init; }
    public string? BankLocation { get; init; }
    public string? BankName { get; init; }
    public string? Bic { get; init; }
    public string? CorrespondentAccount { get; init; }
    public bool? IsDefault { get; init; }
}

public record ContactPerson : Entity
{
    public Counterparty? Agent { get; init; }
    public string? Description { get; init; }
    public string? Email { get; init; }
    public string? ExternalCode { get; init; }
    public string? Phone { get; init; }
    public string? Position { get; init; }
}