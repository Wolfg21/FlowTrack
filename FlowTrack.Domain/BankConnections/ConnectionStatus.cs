using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.BankConnections;
public enum ConnectionStatus
{
    Active,
    Error,
    Revoked,
    Pending
}
