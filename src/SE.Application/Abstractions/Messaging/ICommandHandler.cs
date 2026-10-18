using SE.Application.Common.Response;

namespace SE.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken ct);
}