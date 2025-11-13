using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.BankConnections.Events;
using FlowTrack.Domain.Institution;
using FlowTrack.Domain.Users;

namespace FlowTrack.Domain.BankConnections;

public sealed class BankConnection : Entity<BankConnectionId>
{
    private readonly List<Product> _consentedProducts;

    private BankConnection(
        BankConnectionId id,
        UserId userId,
        InstitutionId institutionId,
        PlaidItemId plaidItemId,
        DateTime connectedOnUtc,
        List<Product> consentedProducts,
        DateTime? lastSuccessfulSyncOnUtc,
        DateTime? consentExpirationOnUtc)
        : base(id)
    {
        UserId = userId;
        InstitutionId = institutionId;
        PlaidItemId = plaidItemId;

        ConnectedOnUtc = connectedOnUtc;

        _consentedProducts = new List<Product>(consentedProducts);

        LastSuccessfulSyncOnUtc = lastSuccessfulSyncOnUtc;
        ConsentExpirationOnUtc = consentExpirationOnUtc;
    }

    public UserId UserId { get; private set; }
    public InstitutionId InstitutionId { get; private set; }
    public PlaidItemId PlaidItemId { get; private set; }

    public DateTime ConnectedOnUtc { get; private set; }
    public DateTime? LastSuccessfulSyncOnUtc { get; private set; }
    public DateTime? ConsentExpirationOnUtc { get; private set; }

    public IReadOnlyList<Product> ConsentedProducts => _consentedProducts.AsReadOnly();

    public bool IsConsentExpired() =>
        ConsentExpirationOnUtc.HasValue && ConsentExpirationOnUtc.Value < DateTime.UtcNow;

    public static BankConnection Create(
        UserId userId,
        InstitutionId institutionId,
        PlaidItemId plaidItemId,
        DateTime utcNow,
        List<Product> consentedProducts,
        DateTime? lastSuccessfulSyncOnUtc,
        DateTime? consentExpirationOnUtc)
    {
        var id = new BankConnectionId(Guid.NewGuid());

        var bankConnection = new BankConnection(
            id,
            userId,
            institutionId,
            plaidItemId,
            utcNow,
            consentedProducts,
            lastSuccessfulSyncOnUtc,
            consentExpirationOnUtc);

        bankConnection.RaiseDomainEvent(new BankConnectionCreatedDomainEvent(bankConnection.Id));

        return bankConnection;
    }
}
