using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Users;

namespace FlowTrack.Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var firstName = new FirstName(request.FirstName);
        var lastName = new LastName(request.LastName);
        var email = new Email(request.Email);

        var user = User.Create(firstName, lastName, email);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
