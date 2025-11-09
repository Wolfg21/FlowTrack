using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts.Events;
using System;

namespace FlowTrack.Domain.Accounts;

public sealed class Account : Entity
{
    private Account(
        Guid id,
        Guid plaidItemId,
        string plaidAccountId,
        string name,
        AccountType type,
        decimal currentBalance,
        string currencyCode,
        string? officialName,
        string? subtype,
        decimal? availableBalance) : base(id)
    {
        PlaidItemId = plaidItemId;
        PlaidAccountId = plaidAccountId;
        Name = name;
        Type = type;
        CurrentBalance = currentBalance;
        CurrencyCode = currencyCode;
        OfficialName = officialName;
        Subtype = subtype;
        AvailableBalance = availableBalance;
        IsActive = true;
    }

    private Account()
    {
    }

    public Guid PlaidItemId { get; private set; }

    public string PlaidAccountId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? OfficialName { get; private set; }

    public AccountType Type { get; private set; }

    public string? Subtype { get; private set; }

    public decimal CurrentBalance { get; private set; }

    public decimal? AvailableBalance { get; private set; }

    public string CurrencyCode { get; private set; } = "USD";

    public bool IsActive { get; private set; }

    public DateTime? LastSyncedAt { get; private set; }

    public static Account Create(
        Guid plaidItemId,
        string plaidAccountId,
        string name,
        AccountType type,
        decimal currentBalance,
        string currencyCode,
        string? officialName = null,
        string? subtype = null,
        decimal? availableBalance = null)
    {
        var account = new Account(
            Guid.NewGuid(),
            plaidItemId,
            plaidAccountId,
            name,
            type,
            currentBalance,
            currencyCode,
            officialName,
            subtype,
            availableBalance);

        account.RaiseDomainEvent(new AccountCreatedDomainEvent(account.Id, plaidItemId));

        return account;
    }

    public void UpdateBalances(decimal currentBalance, decimal? availableBalance, DateTime? syncedAt = null)
    {
        CurrentBalance = currentBalance;
        AvailableBalance = availableBalance;
        LastSyncedAt = syncedAt ?? DateTime.UtcNow;
        Touch();
    }

    public void Rename(string name, string? officialName)
    {
        Name = name;
        OfficialName = officialName;
        Touch();
    }

    public void SetSubtype(string? subtype)
    {
        Subtype = subtype;
        Touch();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        Touch();
    }
}
