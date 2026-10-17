namespace SE.Infrastructure.Persistence.Seeding;

public interface ISeeder
{
    int Order { get; }   // lower runs first
    Task SeedAsync(CancellationToken ct = default);
}