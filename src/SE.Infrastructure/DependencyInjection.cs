using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SE.Application.Abstractions.Persistence;
using SE.Application.Abstractions.Security;
using SE.Infrastructure.Persistence;
using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Repositories;
using SE.Infrastructure.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString).EnableSensitiveDataLogging()
        );
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return services;
    }
}
