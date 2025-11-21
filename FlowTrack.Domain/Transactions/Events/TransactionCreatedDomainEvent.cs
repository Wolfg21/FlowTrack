using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;

namespace FlowTrack.Domain.Transactions.Events;
public sealed record TransactionCreatedDomainEvent(TransactionId TransactionId) : IDomainEvent;