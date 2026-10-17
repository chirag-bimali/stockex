using Microsoft.EntityFrameworkCore;

namespace SE.Infrastructure.Persistence.Configurations;

public class AppDbContext(
    DbContextOptions<AppDbContext> options)
    : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Finds every configuration class in this project automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

}