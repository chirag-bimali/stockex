using SE.Application.Abstractions.Persistence;
using SE.Infrastructure.Persistence.Configurations;

namespace SE.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct) => await db.SaveChangesAsync(ct);

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        try
        {
            var result = await action();
            await transaction.CommitAsync(ct);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}
