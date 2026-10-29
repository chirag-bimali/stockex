using SE.Domain.Entities;

namespace SE.Application.Features.Users.GetAllUser;

public sealed record GetAllUserResult(IReadOnlyCollection<GetAllUserItem> Users)
{
    public GetAllUserResult(IEnumerable<User> users)
        : this(users.Select(GetAllUserItem.FromUser).ToArray())
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
    DateTimeOffset CreatedAt
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
            user.CreatedAt
        );
}