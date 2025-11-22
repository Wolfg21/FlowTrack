using FlowTrack.Domain.Accounts;
using FlowTrack.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Transactions;
public interface ITransactionRepository
{
    Task<Transaction?> GetByPlaidIdAsync(ExternalId plaidTransactionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetByAccountAsync(
        AccountId accountId,
        DateRange? dateRange = default,
        CancellationToken cancellationToken = default);

    Task UpsertPageAsync(
        IEnumerable<Transaction> added,
        IEnumerable<Transaction> modified,
        IEnumerable<ExternalId> removed,
        string nextCursor,
        CancellationToken cancellationToken = default);
    Task RemoveRangeAsync(IEnumerable<ExternalId> removed, CancellationToken cancellationToken = default);
}
