namespace SE.Infrastructure.Persistence.Seeding;

public class DatabaseSeeder(IEnumerable<ISeeder> seeders)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var seeder in seeders.OrderBy(s => s.Order))
            await seeder.SeedAsync(ct);
    }
}