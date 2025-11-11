namespace FlowTrack.Application.Users.Queries.GetUser;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
