using Microsoft.AspNetCore.Mvc;
using SE.API.Controllers;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
using SE.Application.Features.Users.CreateUser;

[ApiController]
[Route("user")]
public class UserController(ICommandHandler<CreateUserCommand, Guid> createUser) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken ct)
    {
        var result = await createUser.Handle(command, ct);
        return FromCreated(result, value => $"/api/user/{value}");
    }
}
