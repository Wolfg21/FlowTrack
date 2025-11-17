using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Institutions.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Institutions
{
    public sealed class Institution : Entity<InstitutionId>
    {
        private Institution(InstitutionId id, InstitutionName name) : base(id)
        {
            Name = name;
        }

        public InstitutionName Name { get; private set; }

        public static Institution Create(InstitutionName name)
        {
            var instituitionId = new InstitutionId(Guid.NewGuid());

            var institution = new Institution(instituitionId, name);

            institution.RaiseDomainEvent(new InstitutionCreatedDomainEvent(instituitionId));

            return institution;
        }
    }
}
