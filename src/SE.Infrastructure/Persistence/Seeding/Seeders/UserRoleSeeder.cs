using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Security;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Seeding;

public class UserRoleSeeder(AppDbContext db) : ISeeder
{
    public int Order => 20;
    public static readonly (Guid UserId, int RoleId)[] UserRoles =
    [
        (Guid.Parse("00000000-0000-0000-0000-000000000001"), 1),
    ];

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var predicate = PredicateBuilder.New<UserRole>(false);
        foreach (var (userId, roleId) in UserRoles)
        {
            var id = userId;
            var role = roleId;
            predicate = predicate.Or(ur => ur.UserId == id && ur.RoleId == role);
        }
        var existingUserRoles = await db.UserRoles.AsExpandable().Where(predicate).ToListAsync(ct);

        if (existingUserRoles.Count == UserRoles.Length)
            return;

        foreach (var (userId, roleId) in UserRoles)
        {
            if (!existingUserRoles.Any(ur => ur.UserId == userId && ur.RoleId == roleId))
            {
                var userRole = new UserRole(userId, roleId);
                db.UserRoles.Add(userRole);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
