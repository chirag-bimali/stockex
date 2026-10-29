using SE.Domain.Entities;

namespace SE.Application.Abstractions.Persistence;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct);
    Task<(IReadOnlyList<User> Users, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? role,
        bool? isActive,
        bool? isEmailVerified,
        CancellationToken ct
    );
}
