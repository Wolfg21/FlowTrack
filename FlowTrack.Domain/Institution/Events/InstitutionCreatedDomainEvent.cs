using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Institution.Events;
public sealed record InstitutionCreatedDomainEvent(InstitutionId InstitutionId) : IDomainEvent;