using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Seeding;

public class UserSeeder(AppDbContext db) : ISeeder
{
    public int Order => 1;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await db.SaveChangesAsync(ct);
    }
}