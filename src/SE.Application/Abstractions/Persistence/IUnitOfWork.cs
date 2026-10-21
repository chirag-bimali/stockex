namespace SE.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken ct);
}
