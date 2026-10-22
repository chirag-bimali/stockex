using Application.Abstractions.Security;
using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Abstractions.Security;
using SE.Application.Common.Response;
using SE.Application.Common.Security;
using SE.Domain.Entities;

namespace SE.Application.Features.Users.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken
    )
    {
        // Log authenticated user information
        Console.WriteLine($"Authenticated User ID: {_currentUser.Id}");
        Console.WriteLine($"Authenticated User Email: {_currentUser.Email}");
        Console.WriteLine($"Authenticated User Roles: {string.Join(", ", _currentUser.Roles)}");

        if (!_currentUser.IsAuthenticated)
        {
            return Error.Forbidden(
                "user.not_authenticated",
                "User must be authenticated to create a new user."
            );
        }
        List<Role> roles = new List<Role>();
        foreach (var roleName in command.Roles)
        {
            var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);
            if (role is null)
            {
                return Error.NotFound("role.not_found", $"Role '{role}' does not exist.");
            }
            roles.Add(role);
        }

        var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser is not null)
        {
            return Error.Conflict("user.email_exists", "User with the same email already exists.");
        }

        var user = new User(command.Email, _passwordHasher.HashPassword(command.Password), command.FirstName, command.LastName);
        foreach (var role in roles)
            user.AssignRole(role);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}
