namespace FlowTrack.Application.BankConnections.GenerateLinkTokens;

public record LinkTokenResponse(string LinkToken, DateTime ExpiresAtUtc);