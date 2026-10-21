using LinqKit;
using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Persistence;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Repositories;

public class UserRoleRepository(AppDbContext db) : Repository<UserRole>(db), IUserRoleRepository
{
    public async Task<List<UserRole>> GetByPairsAsync(
        IReadOnlyCollection<(Guid UserId, int RoleId)> pairs,
        CancellationToken ct
    )
    {
        if (pairs.Count == 0)
            return [];

        var predicate = PredicateBuilder.New<UserRole>(false);
        foreach (var (userId, roleId) in pairs)
        {
            var id = userId;
            var role = roleId;
            predicate = predicate.Or(ur => ur.UserId == id && ur.RoleId == role);
        }

        return await Db.UserRoles.AsExpandable().Where(predicate).ToListAsync(ct);
    }
}
