using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.BankConnections.Events;
using FlowTrack.Domain.Institution;
using FlowTrack.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.BankConnection;
public sealed class BankConnection : Entity<BankConnectionId>
{
    private BankConnection(
        BankConnectionId id,
        UserId userId,
        InstitutionId institutionId,
        PlaidItemId plaidItemId,
        DateTime createdOnUtc,
        DateTime? lastSuccessfulSyncOnUtc,
        DateTime? consentExpirationOnUtc)
        : base(id)
    {
        UserId = userId;
        InstitutionId = institutionId;
        PlaidItemId = plaidItemId;
        CreatedOnUtc = createdOnUtc;
        LastSuccessfulSyncOnUtc = lastSuccessfulSyncOnUtc;
        ConsentExpirationOnUtc = consentExpirationOnUtc;
    }

    public UserId UserId { get; private set; }
    public InstitutionId InstitutionId { get; private set; }
    public PlaidItemId PlaidItemId { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? LastSuccessfulSyncOnUtc { get; private set; }
    public DateTime? ConsentExpirationOnUtc { get; private set; }

    public bool IsConsentExpired() =>
        ConsentExpirationOnUtc.HasValue && ConsentExpirationOnUtc.Value < DateTime.UtcNow;

    public static BankConnection Create(
        UserId userId,
        PlaidItemId plaidItemId,
        InstitutionId institutionId,
        DateTime? lastSuccessfulSyncOnUtc = null,
        DateTime? consentExpirationOnUtc = null)
    {
        var id = new BankConnectionId(Guid.NewGuid());

        var bankConnection = new BankConnection(
            id,
            userId,
            institutionId,
            plaidItemId,
            DateTime.UtcNow,
            lastSuccessfulSyncOnUtc,
            consentExpirationOnUtc);

        bankConnection.RaiseDomainEvent(new BankConnectionCreatedDomainEvent(bankConnection.Id));

        return bankConnection;
    }
}
