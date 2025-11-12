using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.Institution;
using FlowTrack.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.BankConnection;
public sealed class BankConnection : Entity<BankConnectionId>
{
    BankConnection(BankConnectionId id, UserId userId)
        : base(id)
    {
        UserId = userId;
    }

    public UserId UserId { get; }
    public PlaidItemId PlaidItemId { get; }
    public InstitutionId InstitutionId { get; }

}
