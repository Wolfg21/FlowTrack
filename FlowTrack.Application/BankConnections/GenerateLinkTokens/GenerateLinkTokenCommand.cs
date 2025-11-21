using FlowTrack.Application.Abstractions.Messaging;

namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;
public record GenerateLinkTokenCommand(Guid UserId) : ICommand<LinkTokenResponse>;
