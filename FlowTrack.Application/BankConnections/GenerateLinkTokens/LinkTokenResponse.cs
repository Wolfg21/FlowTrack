namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;

public sealed record LinkTokenResponse(string LinkToken, DateTime ExpiresAtUtc);