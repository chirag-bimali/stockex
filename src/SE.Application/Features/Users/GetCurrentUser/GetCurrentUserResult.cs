namespace SE.Application.Features.Users.GetCurrentUser;

public record GetCurrentUserResult(
    Guid Id,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Permissions
);
