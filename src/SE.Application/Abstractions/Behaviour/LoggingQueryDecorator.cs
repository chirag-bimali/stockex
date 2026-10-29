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
        ILogger<LoggingQueryDecorator<TQuery, TResult>> logger
    )
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResult>> Handle(TQuery command, CancellationToken ct)
    {
        var name = typeof(TQuery).Name;
        _logger.LogInformation("Handling {Query}", name);
        try
        {
            var result = await _inner.Handle(command, ct);

            result.Switch(
                value => _logger.LogInformation("Handled {Query}", name),
                err => _logger.LogWarning("{Query} failed: {ErrorCode}", name, err.Error.Code)
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Query} failed", name);
            throw;
        }
    }
}
