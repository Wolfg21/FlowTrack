using FlowTrack.Domain.Abstractions;
using System;

namespace FlowTrack.Domain.Categories.Events;

public sealed record CategoryCreatedDomainEvent(Guid CategoryId, Guid? UserId, bool IsSystemCategory) : IDomainEvent;
