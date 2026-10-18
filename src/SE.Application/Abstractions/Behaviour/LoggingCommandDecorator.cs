using Microsoft.Extensions.Logging;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;

namespace SE.Application.Abstractions.Behaviour;

public class LoggingCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;

    private readonly ILogger<LoggingCommandDecorator<TCommand, TResult>> _logger;

    public LoggingCommandDecorator(
        ICommandHandler<TCommand, TResult> inner,
        ILogger<LoggingCommandDecorator<TCommand, TResult>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TResult>> Handle(TCommand command, CancellationToken ct)
    {
        var name = typeof(TCommand).Name;
        _logger.LogInformation("Handling {Command}", name);
        try
        {
            var result = await _inner.Handle(command, ct);

            result.Switch(
                value => _logger.LogInformation("Handled {Command}", name),
                err => _logger.LogWarning("{Command} failed: {ErrorCode}", name, err.Error.Code));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Command} failed", name);
            throw;
        }
    }
}