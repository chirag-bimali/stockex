using Microsoft.EntityFrameworkCore;
using SE.Application.Common.Security;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Seeding.Seeders;

public sealed class PermissionSeeder(AppDbContext db) : ISeeder
{
    public int Order => 3;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var existing = await db.Set<Permission>().ToListAsync(ct);

        foreach (var name in Permissions.All.Where(n => existing.All(e => e.Name != n)))
        {
            var permission = new Permission(name);
            db.Add(permission);
            existing.Add(permission);
        }
        await db.SaveChangesAsync(ct);

        var adminRoles = await db.Set<Role>()
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Name == "System Admin", ct);

        if (adminRoles is not null)
        {
            foreach (
                var p in existing.Where(p =>
                    adminRoles.RolePermissions.All(rp => rp.PermissionId != p.Id)
                )
            )
            {
                if (p.Name == Permissions.Trader.Access)
                    continue;

                adminRoles.RolePermissions.Add(new RolePermission(adminRoles.Id, p.Id));
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
