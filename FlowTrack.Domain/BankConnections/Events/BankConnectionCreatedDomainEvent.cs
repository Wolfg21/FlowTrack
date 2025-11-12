using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.BankConnections.Events;

public sealed record BankConnectionCreatedDomainEvent(BankConnectionId BankConnectionId) : IDomainEvent;
