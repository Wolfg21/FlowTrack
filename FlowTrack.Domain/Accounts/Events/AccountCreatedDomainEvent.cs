using FlowTrack.Domain.Abstractions;
using System;

namespace FlowTrack.Domain.Accounts.Events;

public sealed record AccountCreatedDomainEvent(Guid AccountId, Guid PlaidItemId) : IDomainEvent;
