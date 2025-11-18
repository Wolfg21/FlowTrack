using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts.Events;
using FlowTrack.Domain.BankConnections;

namespace FlowTrack.Domain.Accounts;
public sealed class Account : Entity<AccountId>
{
    private Account(
        AccountId id,
        string plaidAccountId,
        BankConnectionId bankConnectionId,
        Balances balances,
        HolderCategory holderCategory,
        Mask mask,
        Name name,
        OfficialName officialName,
        AccountType type,
        AccountSubtype subtype) 
        : base(id)
    {
        PlaidAccountId = plaidAccountId;
        Balances = balances;
        BankConnectionId = bankConnectionId;
        HolderCategory = holderCategory;
        Mask = mask;
        Name = name;
        OfficialName = officialName;
        Type = type;
        Subtype = subtype;
    }

    public string PlaidAccountId { get; private set; }

    public BankConnectionId BankConnectionId { get; private set; }
    
    public Balances Balances { get; private set; }
    
    public HolderCategory? HolderCategory { get; private set; } 
    
    public Mask? Mask { get; private set; } 
   
    public Name Name { get; private set; } 
    
    public OfficialName? OfficialName { get; private set; }
    
    public AccountType Type { get; private set; }
    
    public AccountSubtype? Subtype { get; private set; }

    public void SetBalance(Balances balances) => Balances = balances;
    
    public static Account Create(
        string plaidAccountId,
        Balances balances,
        BankConnectionId bankConnectionId,
        HolderCategory holderCategory,
        Mask mask,
        Name name,
        OfficialName officialName,
        AccountType type,
        AccountSubtype subtype)
    {
        var id = new AccountId(Guid.NewGuid());

        var account = new Account(
            id, 
            plaidAccountId,
            bankConnectionId,
            balances,
            holderCategory,
            mask,
            name,
            officialName,
            type,
            subtype);

        account.RaiseDomainEvent(new AccountCreatedDomainEvent(account.Id));

        return account;
    }
}
