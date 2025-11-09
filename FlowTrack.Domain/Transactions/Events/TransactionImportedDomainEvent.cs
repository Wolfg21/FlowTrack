using FlowTrack.Domain.Abstractions;
using System;

namespace FlowTrack.Domain.Transactions.Events;

public sealed record TransactionImportedDomainEvent(Guid TransactionId, Guid AccountId) : IDomainEvent;
