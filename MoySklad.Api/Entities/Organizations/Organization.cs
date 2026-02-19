using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Products;

namespace MoySklad.Api.Entities.Organizations;

public record Organization : BaseOrganizationalEntity
{
    public string? LegalFirstName { get; init; }
    public string? LegalLastName { get; init; }
    public string? Director { get; init; }
    public string? DirectorPosition { get; init; }
    public string? ChiefAccountant { get; init; }
    
    public bool? PayerVat { get; init; }
    public bool? PayerVat105 { get; init; }
    public decimal? AdvancePaymentVat { get; init; }
    
    public string? TrackingContractNumber { get; init; }
    public string? TrackingContractDate { get; init; }
    public string? CertificateNumber { get; init; }
    public string? CertificateDate { get; init; }
    
    public bool? IsEgaisEnabled { get; init; }
    public string? FsrarId { get; init; }
    public string? UtmUrl { get; init; }
    
    public Entity? BonusProgram { get; init; }
    
    public string? Orgnip { get; init; }
}

public record Counterparty : BaseOrganizationalEntity
{
    public List<string>? Tags { get; init; }
    public List<State>? States { get; init; }
    
    public ContactPerson? ContactPerson { get; init; }
    public List<CounterpartyNote>? Notes { get; init; }
    
    public int? BonusPoints { get; init; }
    public Entity? BonusProgram { get; init; }
    public string? DiscountCardNumber { get; init; }
    public List<Dictionary<string, object>>? Discounts { get; init; }
    public Entity? PriceType { get; init; }
    
    public string? SyncId { get; init; }
}

public record CounterpartyNote : Entity
{
    public Counterparty? Agent {  get; init; }
    public Employee? Author {  get; init; }
    public Entity? AuthorApplication { get; init; }
    public string? Description { get; init; }
}

public record Store : Entity
{
    public string? Address { get; init; }
    public OrganizationAddress? AddressFull { get; init; }
    public bool? Archived { get; init; }
    public List<EntityAttribute>? Attributes { get; init; }
    public string? Code { get; init; }
    public string? Description { get; init; }
    public string? ExternalCode { get; init; }
    public Group? Group { get; init; }
    public Employee? Owner { get; init; }
    public Store? Parent { get; init; }
    public string? PathName { get; init; }
    public bool? Shared { get; init; }
    public List<Dictionary<string, object>>? Zones { get; init; }
    public List<Dictionary<string, object>>? Slots { get; init; }
}