using FlowTrack.Application.Abstractions.Messaging;

namespace FlowTrack.Application.Users.Queries.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;
