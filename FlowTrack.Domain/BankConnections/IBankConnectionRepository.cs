using FlowTrack.Domain.Shared;
using FlowTrack.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.BankConnections;
public interface IBankConnectionRepository
{
    Task<BankConnection?> GetByIdAsync(BankConnectionId id, CancellationToken cancellationToken = default);
    Task<BankConnection?> GetByPlaidIdAsync(ExternalId plaidItemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankConnection>> GetByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    void Add(BankConnection connection);
    Task UpdateCursorAsync(BankConnectionId id, string? nextCursor, CancellationToken cancellationToken = default);
}
