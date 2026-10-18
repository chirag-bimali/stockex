using Microsoft.Extensions.Logging;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;
namespace SE.Application.Abstractions.Behaviour;
public class LoggingQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _inner;
    private readonly ILogger<LoggingQueryDecorator<TQuery, TResult>> _logger;

    public LoggingQueryDecorator(
        IQueryHandler<TQuery, TResult> inner,
        ILogger<LoggingQueryDecorator<TQuery, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResult>> Handle(TQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Running {Query}", typeof(TQuery).Name);
        return await _inner.Handle(query, ct);
    }
}