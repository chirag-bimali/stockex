using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using SE.Application.Common.Response;
using SE.API.Common.Response;

namespace SE.API.Middlewares;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken ct)
    {
        Error error;
        int status;

        if (exception is ValidationException validation)
        {
            var details = validation.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray());

            error = Error.Validation("Validation.Failed", "One or more validation errors occurred.", details);
            status = StatusCodes.Status400BadRequest;
        }
        else
        {
            logger.LogError(exception, "Unhandled exception");
            error = Error.Failure("Server.Error", "Something went wrong. Please try again later.");
            status = StatusCodes.Status500InternalServerError;
        }

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(error), ct);
        return true;
    }
}