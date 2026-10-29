using Microsoft.AspNetCore.Mvc;
using SE.API.Controllers;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
using SE.Application.Features.Authentication.Login;
using SE.Application.Features.Authentication.Register;

[ApiController]
public class AuthController : ApiControllerBase
{
    private readonly ICommandHandler<LoginCommand, LoginResult> _login;
    private readonly ICommandHandler<RegisterCommand, Guid> _register;

    public AuthController(
        ICommandHandler<LoginCommand, LoginResult> login,
        ICommandHandler<RegisterCommand, Guid> register
    )
    {
        _login = login;
        _register = register;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await _login.Handle(command, ct);
        return FromResult(result);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(RegisterCommand command, CancellationToken ct)
    {
        var result = await _register.Handle(command, ct);
        return FromCreated(result, value => "/api/auth/login");
    }
}
