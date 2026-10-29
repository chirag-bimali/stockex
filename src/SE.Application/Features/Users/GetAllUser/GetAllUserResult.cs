using SE.Domain.Entities;

namespace SE.Application.Features.Users.GetAllUser;

public sealed record GetAllUserResult(
    IReadOnlyCollection<GetAllUserItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public GetAllUserResult(
        IEnumerable<User> users,
        int pageNumber,
        int pageSize,
        int totalCount
    )
        : this(
            users.Select(GetAllUserItem.FromUser).ToArray(),
            pageNumber,
            pageSize,
            totalCount
        )
    {
    }
}

public sealed record GetAllUserItem(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    bool IsEmailVerified,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<string> Roles
)
{
    public static GetAllUserItem FromUser(User user) =>
        new(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.IsActive,
            user.IsEmailVerified,
            user.CreatedAt,
            user.UserRoles
                .Where(userRole => userRole.Role is not null)
                .Select(userRole => userRole.Role.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToArray()
        );
}