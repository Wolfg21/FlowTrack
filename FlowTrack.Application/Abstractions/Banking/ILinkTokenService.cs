using FlowTrack.Application.BankConnections.GenerateLinkTokens;
using FlowTrack.Domain.Users;

namespace FlowTrack.Application.Abstractions.Banking;

public interface ILinkTokenService
{
    Task<LinkTokenResponse> GenerateLinkTokenAsync(UserId userId, CancellationToken cancellationToken = default);
}