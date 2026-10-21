using Microsoft.EntityFrameworkCore;
using SE.Domain.Entities;

namespace SE.Infrastructure.Persistence.Configurations;

public class AppDbContext(
    DbContextOptions<AppDbContext> options)
    : DbContext(options)
{

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Finds every configuration class in this project automatically
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
    }

}