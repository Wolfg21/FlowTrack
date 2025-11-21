using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Abstractions;

namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;
internal sealed class GenerateLinkTokenCommandHandler : ICommandHandler<GenerateLinkTokenCommand, LinkTokenResponse>
{
    public Task<Result<LinkTokenResponse>> Handle(GenerateLinkTokenCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
