using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts;
using FlowTrack.Domain.PlaidItems.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowTrack.Domain.PlaidItems;

public sealed class PlaidItem : Entity
{
    private readonly List<Account> _accounts = new();

    private PlaidItem(
        Guid id,
        Guid userId,
        string plaidItemId,
        string plaidAccessToken,
        string institutionId,
        string institutionName) : base(id)
    {
        UserId = userId;
        PlaidItemId = plaidItemId;
        PlaidAccessToken = plaidAccessToken;
        InstitutionId = institutionId;
        InstitutionName = institutionName;
        Status = PlaidItemStatus.Connected;
    }

    private PlaidItem()
    {
    }

    public Guid UserId { get; private set; }

    public string PlaidItemId { get; private set; } = string.Empty;

    public string PlaidAccessToken { get; private set; } = string.Empty;

    public string InstitutionId { get; private set; } = string.Empty;

    public string InstitutionName { get; private set; } = string.Empty;

    public PlaidItemStatus Status { get; private set; }

    public DateTime? LastSyncedAt { get; private set; }

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();

    public static PlaidItem Connect(
        Guid userId,
        string plaidItemId,
        string plaidAccessToken,
        string institutionId,
        string institutionName)
    {
        var plaidItem = new PlaidItem(
            Guid.NewGuid(),
            userId,
            plaidItemId,
            plaidAccessToken,
            institutionId,
            institutionName);

        plaidItem.RaiseDomainEvent(new PlaidItemConnectedDomainEvent(plaidItem.Id, userId));

        return plaidItem;
    }

    public void UpdateAccessToken(string plaidAccessToken)
    {
        if (string.Equals(PlaidAccessToken, plaidAccessToken, StringComparison.Ordinal))
        {
            return;
        }

        PlaidAccessToken = plaidAccessToken;
        Touch();
    }

    public void SetStatus(PlaidItemStatus status)
    {
        if (Status == status)
        {
            return;
        }

        Status = status;
        Touch();
    }

    public void MarkSynced(DateTime syncedAt)
    {
        LastSyncedAt = syncedAt;
        Touch();
    }

    public void AddAccount(Account account)
    {
        if (_accounts.Any(existing => existing.Id == account.Id))
        {
            return;
        }

        _accounts.Add(account);
        Touch();
    }
}
