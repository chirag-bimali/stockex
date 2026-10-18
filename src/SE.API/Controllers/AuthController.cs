using Microsoft.AspNetCore.Mvc;
using SE.API.Controllers;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
using SE.Application.Features.Authentication.Login;

[ApiController]
[Route("auth")]
public class AuthController : ApiControllerBase
{
    private readonly ICommandHandler<LoginCommand, LoginResult> _login;

    public AuthController(ICommandHandler<LoginCommand, LoginResult> login)
        => _login = login;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await _login.Handle(command, ct);
        return FromCreated(result, value => "/api/auth/login");
    }
}