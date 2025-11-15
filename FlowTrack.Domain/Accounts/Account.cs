using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Accounts.Events;
using FlowTrack.Domain.BankConnections;

namespace FlowTrack.Domain.Accounts;
public sealed class Account : Entity<AccountId>
{
    private Account(
        AccountId id,
        BankConnectionId bankConnectionId,
        Balances balances,
        HolderCategory holderCategory,
        Mask mask,
        Name name,
        OfficialName officialName,
        Type type,
        Subtype subtype) 
        : base(id)
    {
        Balances = balances;
        BankConnectionId = bankConnectionId;
        HolderCategory = holderCategory;
        Mask = mask;
        Name = name;
        OfficialName = officialName;
        Type = type;
        Subtype = subtype;
    }

    public BankConnectionId BankConnectionId { get; private set; }
    
    public Balances Balances { get; private set; }
    
    public HolderCategory? HolderCategory { get; private set; } 
    
    public Mask? Mask { get; private set; } 
   
    public Name Name { get; private set; } 
    
    public OfficialName? OfficialName { get; private set; }
    
    public Type Type { get; private set; }
    
    public Subtype? Subtype { get; private set; }

    public void SetBalance(Balances balances) => Balances = balances;
    
    public static Account Create(
        Balances balances,
        BankConnectionId bankConnectionId,
        HolderCategory holderCategory,
        Mask mask,
        Name name,
        OfficialName officialName,
        Type type,
        Subtype subtype)
    {
        var id = new AccountId(Guid.NewGuid());

        var account = new Account(
            id, 
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
