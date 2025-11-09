using FlowTrack.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.User.Events;
public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;