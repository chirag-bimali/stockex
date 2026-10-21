using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Abstractions.Security;
using SE.Application.Common.Response;
using SE.Domain.Entities;

namespace SE.Application.Features.Authentication.Login;

public class LoginHandler(
    IUserRepository repository,
    IJwtTokenService jwtTokenService,
    IPasswordHasher passwordHasher
) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<Result<LoginResult>> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await repository.GetByEmailAsync(command.Email, ct);

        if (user is null)
        {
            return Error.Forbidden("auth.invalid_credentials", "Invalid email or password.");
        }

        if (passwordHasher.VerifyPassword(command.Password, user.PasswordHash) is false)
        {
            return Error.Forbidden("auth.invalid_credentials", "Invalid email or password.");
        }

        var generatedToken = jwtTokenService.GenerateAccessToken(user, user.UserRoles.Select(r => r.Role.Name));

        return new LoginResult(generatedToken.Token, generatedToken.ExpiresAt);
    }
}
