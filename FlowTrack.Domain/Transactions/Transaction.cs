using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts;
using FlowTrack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Transactions;
public sealed class Transaction : Entity<TransactionId>
{
    private Transaction(
        TransactionId id,
        AccountId accountId,
        Money amount,
        DateTime date,
        Name name,
        Category? category,
        TransactionType type)
        : base(id)
    {
        AccountId = accountId;
        Amount = amount;
        Date = date;
        Name = name;
        Category = category;
        Type = type;
    }
    public AccountId AccountId { get; private set; }
    public Money Amount { get; private set; }
    public DateTime Date { get; private set; }
    public Name Name { get; private set; }
    public Category? Category { get; private set; }
    public TransactionType Type { get; private set; }
    public static Transaction Create(
        AccountId accountId,
        Money amount,
        DateTime date,
        Name name,
        Category? category,
        TransactionType type)
    {
        var id = new TransactionId(Guid.NewGuid());
        var transaction = new Transaction(
            id,
            accountId,
            amount,
            date,
            name,
            category,
            type);
        return transaction;
    }
}
