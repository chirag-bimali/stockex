using SE.Application.Common.Response;

namespace SE.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResult>
{
    Task<Result<TResult>> Handle(TQuery query, CancellationToken ct);
}