using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Abstractions.Security;
using SE.Application.Common.Response;
using SE.Domain.Entities;

namespace SE.Application.Features.Authentication.Register;

public class RegisterHandler(
    IUserRepository repository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository
) : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken ct)
    {
        var existingUser = await repository.GetByEmailAsync(command.Email, ct);

        if (existingUser is not null)
        {
            return Error.Conflict("auth.email_exists", "Email already exists.");
        }

        var hashedPassword = passwordHasher.HashPassword(command.Password);

        var newUser = new User(command.Email, hashedPassword, command.FirstName, command.LastName);
        newUser.AssignRole(
            await roleRepository.GetByNameAsync("Trader", ct)
                ?? throw new Exception("Default role not found.")
        );

        await repository.AddAsync(newUser, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return newUser.Id;
    }
}
