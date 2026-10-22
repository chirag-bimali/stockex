using SE.Application.Common.Security;

namespace SE.Application.Features.Users.CreateUser;

[RequirePermission(Permissions.Users.Create)]
public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    IEnumerable<string> Roles
);
