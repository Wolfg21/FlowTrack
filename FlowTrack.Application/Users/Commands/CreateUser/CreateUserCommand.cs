using FlowTrack.Application.Abstractions.Messaging;

namespace FlowTrack.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email) : ICommand<Guid>;
