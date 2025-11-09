using FlowTrack.Domain.Abstractions;
using System;

namespace FlowTrack.Domain.PlaidItems.Events;

public sealed record PlaidItemConnectedDomainEvent(Guid PlaidItemId, Guid UserId) : IDomainEvent;
