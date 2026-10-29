using SE.Domain.Entities;

namespace SE.Application.Abstractions.Persistence;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct);
}
