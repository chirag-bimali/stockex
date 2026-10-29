using Microsoft.AspNetCore.Mvc;
using SE.API.Controllers;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
using SE.Application.Features.Users.CreateUser;
using SE.Application.Features.Users.GetAllUser;
using SE.Application.Features.Users.GetCurrentUser;

[ApiController]
public class UserController(
    ICommandHandler<CreateUserCommand, Guid> createUser,
    IQueryHandler<GetAllUserQuery, GetAllUserResult> getAllUsers,
    IQueryHandler<GetCurrentUserQuery, GetCurrentUserResult> getCurrentUser
) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] bool? isEmailVerified = null,
        CancellationToken ct = default
    )
    {
        return FromResult(
            await getAllUsers.Handle(
                new GetAllUserQuery(
                    pageNumber,
                    pageSize,
                    search,
                    role,
                    isActive,
                    isEmailVerified
                ),
                ct
            )
        );
    }

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
