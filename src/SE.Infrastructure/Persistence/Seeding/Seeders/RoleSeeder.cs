using Microsoft.EntityFrameworkCore;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Seeding;

namespace SE.Infrastructure.Persistence.Configurations;

public class RoleSeeder(AppDbContext db) : ISeeder
{
    public int Order => 0;

    public static readonly (int id, string Name, string Description)[] Roles =
    {
        (1, "System Admin", "Monitors system"),
        (2, "Exchange Officer", "Manages exchange"),
        (3, "Company Representative", "Represents a company"),
        (4, "Trader", "Trades stocks"),
    };

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var existingRoles = await db.Roles.ToListAsync(ct);

        foreach (var (id, name, description) in Roles)
        {
            if (!existingRoles.Any(r => r.Id == id))
            {
                var role = new Role(id, name, description);
                db.Roles.Add(role);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
