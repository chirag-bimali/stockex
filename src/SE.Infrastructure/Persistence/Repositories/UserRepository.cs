using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Persistence;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext db) : Repository<User>(db), IUserRepository
{
    public async Task<(IReadOnlyList<User> Users, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? role,
        bool? isActive,
        bool? isEmailVerified,
        CancellationToken ct
    )
    {
        var query = Db.Users
            .AsNoTracking()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(user =>
                user.Email.Contains(searchTerm)
                || user.FirstName.Contains(searchTerm)
                || user.LastName.Contains(searchTerm)
            );
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleName = role.Trim();
            query = query.Where(user =>
                user.UserRoles.Any(userRole => userRole.Role.Name == roleName)
            );
        }

        if (isActive.HasValue)
            query = query.Where(user => user.IsActive == isActive.Value);

        if (isEmailVerified.HasValue)
            query = query.Where(user => user.IsEmailVerified == isEmailVerified.Value);

        var totalCount = await query.CountAsync(ct);
        var users = await query
            .OrderByDescending(user => user.CreatedAt)
            .ThenBy(user => user.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (users, totalCount);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        await Db
            .Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct) =>
        await Db
            .Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
}
