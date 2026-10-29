using Application.Abstractions.Security;
using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Abstractions.Security;
using SE.Application.Common.Response;

namespace SE.Application.Features.Users.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    IPermissionService permissionService
) : IQueryHandler<GetCurrentUserQuery, GetCurrentUserResult>
{
    public async Task<Result<GetCurrentUserResult>> Handle(
        GetCurrentUserQuery query,
        CancellationToken ct
    )
    {
        if (!currentUser.IsAuthenticated)
        {
            return Error.Forbidden(
                "user.not_authenticated",
                "User must be authenticated to get current user information."
            );
        }
        if (currentUser.Id.HasValue is false)
        {
            return Error.NotFound("user.not_found", "Authenticated user email is not available.");
        }
        var userId = currentUser.Id.Value;

        var user = await userRepository.GetByIdWithRolesAsync(userId, ct);

        if (user is null)
        {
            return Error.NotFound("user.not_found", "Authenticated user not found.");
        }

        var roles = user
            .UserRoles.Select(ur => ur.Role.Name)
            .Distinct()
            .OrderBy(name => name)
            .ToArray();

        var permissions = await permissionService.GetPermissionsAsync(userId, ct);

        return new GetCurrentUserResult(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            roles,
            permissions
        );
    }
}
