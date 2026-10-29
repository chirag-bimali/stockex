namespace SE.Application.Features.Users.GetCurrentUser;

public record GetCurrentUserResult(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions
);
