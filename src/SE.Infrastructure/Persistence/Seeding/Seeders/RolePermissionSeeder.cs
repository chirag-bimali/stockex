using Microsoft.EntityFrameworkCore;
using SE.Application.Common.Security;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Seeding.Seeders;

public sealed class RolePermissionSeeder(AppDbContext db) : ISeeder
{
    public int Order => 4;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        // get all permissions
        var permissions = await db.Set<Permission>().ToListAsync(ct);
        var adminRoles = await db.Set<Role>()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == "System Admin", ct);
        var traderRole = await db.Set<Role>()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == "Trader", ct);

        if (adminRoles is null)
        {
            throw new InvalidOperationException("System Admin role not found.");
        }
        if (traderRole is null)
        {
            throw new InvalidOperationException("Trader role not found.");
        }

        if (
            !adminRoles.RolePermissions.Any(rp =>
                rp.PermissionId == permissions.First(p => p.Name == Permissions.Admin.Access).Id
            )
        )
        {
            adminRoles.RolePermissions.Add(
                new RolePermission(
                    adminRoles.Id,
                    permissions.First(p => p.Name == Permissions.Admin.Access).Id
                )
            );
        }

        if (
            !traderRole.RolePermissions.Any(rp =>
                rp.PermissionId == permissions.First(p => p.Name == Permissions.Trader.Access).Id
            )
        )
        {
            traderRole.RolePermissions.Add(
                new RolePermission(
                    adminRoles.Id,
                    permissions.First(p => p.Name == Permissions.Trader.Access).Id
                )
            );
        }

        await db.SaveChangesAsync(ct);
    }
}
