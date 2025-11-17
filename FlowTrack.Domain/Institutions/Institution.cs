using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Institutions.Events;
using FlowTrack.Domain.Shared;
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
        private Institution(
            InstitutionId id,
            InstitutionName name,
            List<Country> countryCodes, 
            List<Product> products,
            InstitutionBranding? branding)
            
            : base(id)
        {
            Name = name;
            CountryCodes = countryCodes;
            Products = products;
            Branding = branding;
        }

        public InstitutionName Name { get; private set; }

        public InstitutionBranding? Branding { get; set; }

        public List<Country> CountryCodes { get; private set; } 

        public List<Product> Products { get; private set; }

        public static Institution Create(
            InstitutionName name,
            List<Country> countryCodes,
            List<Product> products,
            InstitutionBranding? branding)
        {
            var instituitionId = new InstitutionId(Guid.NewGuid());

            var institution = new Institution(
                instituitionId, 
                name,
                countryCodes,
                products,
                branding);

            institution.RaiseDomainEvent(new InstitutionCreatedDomainEvent(instituitionId));

            return institution;
        }
    }
}
