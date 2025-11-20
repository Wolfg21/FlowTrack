using FlowTrack.Domain.BankConnections;
using FlowTrack.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Accounts;
public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(AccountId id, CancellationToken cancellationToken = default);
    Task<Account?> GetByPlaidAccountIdAsync(ExternalId plaidAccountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Account>> GetByConnectionAsync(BankConnectionId bankConnectionId, CancellationToken cancellationToken = default);
    Task UpsertRangeAsync(IEnumerable<Account> accounts, CancellationToken cancellationToken = default);
}
