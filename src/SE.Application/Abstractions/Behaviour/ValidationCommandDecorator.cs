using FluentValidation;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;

public class ValidationCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _inner;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationCommandDecorator(
        ICommandHandler<TCommand, TResult> inner,
        IEnumerable<IValidator<TCommand>> validators
    )
    {
        _inner = inner;
        _validators = validators;
    }

    public async Task<Result<TResult>> Handle(TCommand command, CancellationToken ct)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TCommand>(command);
            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, ct)));
            var failures = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var details = failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());

                return Error.Validation(
                    "Validation.Failed",
                    "One or more validation errors occurred.",
                    details
                );
            }
        }

        return await _inner.Handle(command, ct);
    }
}
