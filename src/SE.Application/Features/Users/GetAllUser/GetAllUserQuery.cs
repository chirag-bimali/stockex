using SE.Application.Common.Security;

namespace SE.Application.Features.Users.GetAllUser;

[RequirePermission(Permissions.Users.Read)]
public sealed record GetAllUserQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    string? Role = null,
    bool? IsActive = null,
    bool? IsEmailVerified = null
);
