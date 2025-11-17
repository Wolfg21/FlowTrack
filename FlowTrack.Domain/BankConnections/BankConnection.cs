using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.BankConnections.Events;
using FlowTrack.Domain.Institutions;
using FlowTrack.Domain.Users;

namespace FlowTrack.Domain.BankConnections;

public sealed class BankConnection : Entity<BankConnectionId>
{
    private BankConnection(
        BankConnectionId id,
        string plaidItemId,
        UserId userId,
        string accessToken,
        InstitutionId institutionId,
        ConnectionStatus status,
        DateTime connectedAtUtc)
        : base(id)
    {
        UserId = userId;
        InstitutionId = institutionId;
        PlaidItemId = plaidItemId;
        AccessToken = accessToken;
        Status = status;
        ConnectedAtUtc = connectedAtUtc;
        TransactionsCursor = null;
    }


    public string PlaidItemId { get; private set; }

    public UserId UserId { get; private set; }

    public string AccessToken { get; private set; }

    public InstitutionId InstitutionId { get; private set; }

    public ConnectionStatus Status { get; private set; }

    public string? TransactionsCursor { get; private set; }

    public DateTime ConnectedAtUtc { get; private set; }

    public static BankConnection Create(
        string plaidItemId,
        UserId userId,
        string accessToken,
        InstitutionId institutionId,
        DateTime? connectedAtUtc = null)
    {
        var id = new BankConnectionId(Guid.NewGuid());

        var bankConnection = new BankConnection(
            id,
            plaidItemId,
            userId,
            accessToken,
            institutionId,
            ConnectionStatus.Active,
            connectedAtUtc ?? DateTime.UtcNow);

        bankConnection.RaiseDomainEvent(new BankConnectionCreatedDomainEvent(bankConnection.Id));

        return bankConnection;
    }

    public void UpdateStatus(ConnectionStatus status) => Status = status;
    public void UpdateTransactionsCursor(string cursor) => TransactionsCursor = cursor;
}
