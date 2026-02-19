using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Documents;

public record PaymentIn : BaseDocument
{
    public Counterparty? Agent { get; init; }
    public OrganizationAccount? AgentAccount { get; init; }
    public Organization? Organization { get; init; }
    public OrganizationAccount? OrganizationAccount { get; init; }
    
    public Contract? Contract { get; init; }
    public string? PaymentPurpose { get; init; }
    public Dictionary<string, object>? SalesChannel { get; init; }
    
    public List<BaseDocument>? Operations { get; init; }
    public Dictionary<string, object>? FactureOut { get; init; }
    
    public decimal? VatSum { get; init; }
    
    public string? IncomingNumber { get; init; }
    public DateTime? IncomingDate { get; init; }
}