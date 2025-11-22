using FlowTrack.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Institutions;
public interface IInstitutionRepository
{
    Task<Institution?> GetByIdAsync(InstitutionId id, CancellationToken cancellationToken = default);
    Task<Institution?> GetByPlaidIdAsync(ExternalId plaidInstitutionId, CancellationToken cancellationToken = default);
    void Add(Institution institution);
}
