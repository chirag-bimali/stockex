using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Persistence;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence.Repositories;

public class Repository<T>(AppDbContext db) : IRepository<T>
    where T : class
{
    protected readonly AppDbContext Db = db;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await Db.Set<T>().FindAsync([id], ct);

    public async Task<List<T>> GetAllAsync(CancellationToken ct) =>
        await Db.Set<T>().ToListAsync(ct);

    public async Task<List<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        return await Db.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct) =>
        await Db.Set<T>().AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct) =>
        await Db.Set<T>().AddRangeAsync(entities, ct);

    public void Remove(T entity) => Db.Set<T>().Remove(entity);
}
