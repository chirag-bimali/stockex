using SE.Domain.Entities;

namespace SE.Application.Abstractions.Persistence;

public interface IUserRoleRepository : IRepository<UserRole>
{
    Task<List<UserRole>> GetByPairsAsync(
        IReadOnlyCollection<(Guid UserId, int RoleId)> pairs,
        CancellationToken ct
    );
}
