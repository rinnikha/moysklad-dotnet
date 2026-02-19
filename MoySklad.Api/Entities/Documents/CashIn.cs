using MoySklad.Api.Entities.Base;
using MoySklad.Api.Entities.Organizations;

namespace MoySklad.Api.Entities.Documents;

public record CashIn : BaseDocument
{
    // Parties
    public Counterparty? Agent { get; init; }
    public OrganizationAccount? AgentAccount { get; init; }
    
    // Payment details
    public Contract? Contract { get; init; }
    public string? PaymentPurpose { get; init; }
    
    // Related operations
    public List<BaseDocument>? Operations { get; init; }
    public Dictionary<string, object>? FactureOut { get; init; }
    
    // Tax
    public decimal? VatSum { get; init; }
    
    public Dictionary<string, object>? RetailShift { get; init; }
}