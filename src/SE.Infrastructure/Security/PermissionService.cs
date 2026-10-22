using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SE.Application.Abstractions.Security;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Security;

public sealed class PermissionService(AppDbContext db, IMemoryCache cache) : IPermissionService
{
    public async Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken ct)
    {
        var permissions = await cache.GetOrCreateAsync(
            $"permissions:{userId}",
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);

                return await (
                    from ur in db.Set<UserRole>()
                    where ur.UserId == userId
                    join rp in db.Set<RolePermission>() on ur.RoleId equals rp.RoleId
                    join p in db.Set<Permission>() on rp.PermissionId equals p.Id
                    select p.Name
                )
                    .Distinct()
                    .ToListAsync(ct);
            }
        );

        return permissions!.Contains(permission);
    }
}
