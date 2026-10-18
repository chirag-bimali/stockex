using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;

namespace SE.Application.Features.Authentication.Login;

public class LoginHandler : ICommandHandler<LoginCommand, LoginResult>
{
    public LoginHandler()
    {
    }

    public async Task<Result<LoginResult>> Handle(LoginCommand command, CancellationToken ct)
    {
        if (command.Email != "admin@se.com" || command.Password != "password")
        {
            var err = Error.UnauthorizedLogin("Auth.InvalidCredentials", "Invalid email or password.", new Dictionary<string, string[]>
            {
                { "Email", new[] { "Provided email is invalid" } },
                { "Password", new[] { "Provided password is invalid" } }
            });
            return err;
        }
        return new LoginResult("token", DateTime.UtcNow.AddHours(1));
    }
}