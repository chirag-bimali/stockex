using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Persistence;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext db) : Repository<User>(db), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        await Db
            .Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, ct);
}
