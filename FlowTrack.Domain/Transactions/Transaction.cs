using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts;
using FlowTrack.Domain.Shared;
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
        string plaidTransactionId,
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

    public string PlaidTransactionId { get; private set; }
    
    public AccountId AccountId { get; private set; }
    
    public Money Amount { get; private set; }
    
    public DateTime Date { get; private set; }
    
    public Name Name { get; private set; }
    
    public MerchantName MerchantName { get; private set; }
    
    public PersonalFinanceCategory? Category { get; private set; }
    
    public PaymentChannel PaymentChannel { get; private set; }
    
    public static Transaction Create(
        string plaidTransactionId,
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

        return transaction;
    }
}
