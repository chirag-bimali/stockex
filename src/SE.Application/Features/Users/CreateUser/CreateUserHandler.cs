using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Common.Response;
using SE.Domain.Entities;

namespace SE.Application.Features.Users.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser is not null)
        {
            return Error.Conflict("user.email_exists", "User with the same email already exists.");
        }
        var user = new User(command.Email, command.Password, command.FirstName, command.LastName);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
