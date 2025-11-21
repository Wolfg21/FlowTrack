using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts;
using FlowTrack.Domain.Shared;
using FlowTrack.Domain.Transactions.Events;
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
        ExternalId plaidTransactionId,
        AccountId accountId,
        Money amount,
        DateTime date,
        Name name,
        MerchantName merchantName,
        PersonalFinanceCategory? category,
        PaymentChannel paymentChannel)
        : base(id)
    {
        PlaidTransactionId = plaidTransactionId;
        AccountId = accountId;
        Amount = amount;
        Date = date;
        Name = name;
        MerchantName = merchantName;
        Category = category;
        PaymentChannel = paymentChannel;
    }

    public ExternalId PlaidTransactionId { get; private set; }
    
    public AccountId AccountId { get; private set; }
    
    public Money Amount { get; private set; }
    
    public DateTime Date { get; private set; }
    
    public Name Name { get; private set; }
    
    public MerchantName MerchantName { get; private set; }
    
    public PersonalFinanceCategory? Category { get; private set; }
    
    public PaymentChannel PaymentChannel { get; private set; }
    
    public static Transaction Create(
        ExternalId plaidTransactionId,
        AccountId accountId,
        Money amount,
        DateTime date,
        Name name,
        MerchantName merchantName,
        PersonalFinanceCategory? category,
        PaymentChannel paymentChannel)
    {
        var id = new TransactionId(Guid.NewGuid());

        var transaction = new Transaction(
            id,
            plaidTransactionId,
            accountId,
            amount,
            date,
            name,
            merchantName,
            category,
            paymentChannel);

        transaction.RaiseDomainEvent(new TransactionCreatedDomainEvent(transaction.Id));

        return transaction;
    }
}
