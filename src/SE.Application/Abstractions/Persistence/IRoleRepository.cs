using SE.Domain.Entities;

namespace SE.Application.Abstractions.Persistence;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
}
