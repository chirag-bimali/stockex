using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Persistence;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Repositories;

namespace SE.Application.Persistence.Repositories;

public class RoleRepository(AppDbContext db) : Repository<Role>(db), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken = default
    )
    {
        return await Db.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
    }
}
