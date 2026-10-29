using Microsoft.AspNetCore.Mvc;
using SE.API.Controllers;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
using SE.Application.Features.Users.CreateUser;
using SE.Application.Features.Users.GetCurrentUser;

[ApiController]
public class UserController(
    ICommandHandler<CreateUserCommand, Guid> createUser,
    IQueryHandler<GetCurrentUserQuery, GetCurrentUserResult> getCurrentUser
) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken ct)
    {
        var result = await createUser.Handle(command, ct);
        return FromCreated(result, value => $"/api/user/{value}");
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        return FromResult(await getCurrentUser.Handle(new GetCurrentUserQuery(), ct));
    }
}
