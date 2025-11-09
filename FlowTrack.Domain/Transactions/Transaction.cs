using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Transactions.Events;
using FlowTrack.Domain.ValueObjects;
using System;

namespace FlowTrack.Domain.Transactions;

public sealed class Transaction : Entity
{
    private Transaction(
        Guid id,
        Guid accountId,
        string plaidTransactionId,
        Money amount,
        TransactionType transactionType,
        DateTime date,
        DateTime? authorizedDate,
        string? merchantName,
        string name,
        bool pending,
        Guid? categoryId,
        string? personalNote,
        bool isRecurring) : base(id)
    {
        AccountId = accountId;
        PlaidTransactionId = plaidTransactionId;
        Amount = amount;
        TransactionType = transactionType;
        Date = date;
        AuthorizedDate = authorizedDate;
        MerchantName = merchantName;
        Name = name;
        Pending = pending;
        CategoryId = categoryId;
        PersonalNote = personalNote;
        IsRecurring = isRecurring;
    }

    private Transaction()
    {
    }

    public Guid AccountId { get; private set; }

    public string PlaidTransactionId { get; private set; } = string.Empty;

    public Money Amount { get; private set; } = null!;

    public TransactionType TransactionType { get; private set; }

    public DateTime Date { get; private set; }

    public DateTime? AuthorizedDate { get; private set; }

    public string? MerchantName { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public bool Pending { get; private set; }

    public Guid? CategoryId { get; private set; }

    public string? PersonalNote { get; private set; }

    public bool IsRecurring { get; private set; }

    public static Transaction Import(
        Guid accountId,
        string plaidTransactionId,
        decimal amount,
        string currencyCode,
        TransactionType transactionType,
        DateTime date,
        DateTime? authorizedDate,
        string? merchantName,
        string name,
        bool pending,
        Guid? categoryId,
        string? personalNote,
        bool isRecurring)
    {
        var normalizedAmount = NormalizeAmount(amount, transactionType);
        var money = Money.From(normalizedAmount, currencyCode);

        var transaction = new Transaction(
            Guid.NewGuid(),
            accountId,
            plaidTransactionId,
            money,
            transactionType,
            date,
            authorizedDate,
            merchantName,
            name,
            pending,
            categoryId,
            personalNote,
            isRecurring);

        transaction.RaiseDomainEvent(new TransactionImportedDomainEvent(transaction.Id, accountId));

        return transaction;
    }

    public void Categorize(Guid? categoryId)
    {
        CategoryId = categoryId;
        Touch();
    }

    public void UpdateNote(string? personalNote)
    {
        PersonalNote = personalNote;
        Touch();
    }

    public void MarkRecurring(bool isRecurring)
    {
        IsRecurring = isRecurring;
        Touch();
    }

    public void SetPending(bool pending)
    {
        Pending = pending;
        Touch();
    }

    public void UpdateDates(DateTime date, DateTime? authorizedDate)
    {
        Date = date;
        AuthorizedDate = authorizedDate;
        Touch();
    }

    public void UpdateMerchant(string? merchantName, string name)
    {
        MerchantName = merchantName;
        Name = name;
        Touch();
    }

    public void UpdateAmount(decimal amount, string currencyCode, TransactionType transactionType)
    {
        var normalizedAmount = NormalizeAmount(amount, transactionType);
        Amount = Money.From(normalizedAmount, currencyCode);
        TransactionType = transactionType;
        Touch();
    }

    private static decimal NormalizeAmount(decimal amount, TransactionType transactionType) =>
        transactionType == TransactionType.Income
            ? Math.Abs(amount)
            : -Math.Abs(amount);
}
