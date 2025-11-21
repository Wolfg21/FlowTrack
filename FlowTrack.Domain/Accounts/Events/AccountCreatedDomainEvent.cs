using FlowTrack.Domain.Abstractions;

namespace FlowTrack.Domain.Accounts.Events;
public sealed record AccountCreatedDomainEvent(AccountId AccountId) : IDomainEvent;
