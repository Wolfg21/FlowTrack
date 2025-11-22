using FlowTrack.Application.Abstractions.Messaging;

namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;
public sealed record GenerateLinkTokenCommand(Guid UserId) : ICommand<LinkTokenResponse>;
