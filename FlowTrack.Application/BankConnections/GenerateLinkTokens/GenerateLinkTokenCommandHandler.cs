using FlowTrack.Application.Abstractions.Banking;
using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;
internal sealed class GenerateLinkTokenCommandHandler : ICommandHandler<GenerateLinkTokenCommand, LinkTokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILinkTokenService _linkTokenService;
    public GenerateLinkTokenCommandHandler(IUserRepository userRepository, ILinkTokenService linkTokenService)
    {
        _userRepository = userRepository;
        _linkTokenService = linkTokenService;
    }
    public async Task<Result<LinkTokenResponse>> Handle(GenerateLinkTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if(user is null)
        {
            return Result.Failure<LinkTokenResponse>(UserErrors.NotFound);
        }

        var token = await _linkTokenService.GenerateLinkTokenAsync(user.Id, cancellationToken);

        return Result.Success(new LinkTokenResponse(token.LinkToken, token.ExpiresAtUtc));
    }
}
