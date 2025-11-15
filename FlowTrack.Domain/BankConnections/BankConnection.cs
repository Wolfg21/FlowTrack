using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.BankConnections.Events;
using FlowTrack.Domain.Institution;
using FlowTrack.Domain.Users;

namespace FlowTrack.Domain.BankConnections;

public sealed class BankConnection : Entity<BankConnectionId>
{
    private BankConnection(
        BankConnectionId id,
        UserId userId,
        InstitutionId institutionId,
        PlaidItemId plaidItemId,
        List<Product> consentedProducts,
        DateTime connectedOnUtc,
        DateTime? consentExpirationOnUtc)
        : base(id)
    {
        UserId = userId;
        InstitutionId = institutionId;
        PlaidItemId = plaidItemId;
        ConsentedProducts = consentedProducts;
        ConnectedOnUtc = connectedOnUtc;
        ConsentExpirationOnUtc = consentExpirationOnUtc;
    }

    public UserId UserId { get; private set; }

    public InstitutionId InstitutionId { get; private set; }
    
    public PlaidItemId PlaidItemId { get; private set; }
    
    public List<Product> ConsentedProducts { get; private set; } = new();
    
    public DateTime ConnectedOnUtc { get; private set; }
    
    public DateTime? ConsentExpirationOnUtc { get; private set; }

    public bool IsConsentExpired() =>
        ConsentExpirationOnUtc.HasValue && ConsentExpirationOnUtc.Value < DateTime.UtcNow;

    public static BankConnection Create(
        UserId userId,
        InstitutionId institutionId,
        PlaidItemId plaidItemId,
        DateTime utcNow,
        List<Product> consentedProducts,
        DateTime? consentExpirationOnUtc)
    {
        var id = new BankConnectionId(Guid.NewGuid());

        var bankConnection = new BankConnection(
            id,
            userId,
            institutionId,
            plaidItemId,
            consentedProducts,
            utcNow,
            consentExpirationOnUtc);

        bankConnection.RaiseDomainEvent(new BankConnectionCreatedDomainEvent(bankConnection.Id));

        return bankConnection;
    }
}
