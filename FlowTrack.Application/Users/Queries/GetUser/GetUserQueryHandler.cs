using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Users;

namespace FlowTrack.Application.Users.Queries.GetUser;

public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        var response = new UserResponse(
            user.Id,
            user.FirstName.Value,
            user.LastName.Value,
            user.Email.Value);

        return Result.Success(response);
    }
}
