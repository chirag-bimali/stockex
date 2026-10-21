// src/PTS.API/Controllers/ApiControllerBase.cs
using Microsoft.AspNetCore.Mvc;
using SE.API.Common.Response;
using SE.Application.Abstractions.Messaging;
using SE.Application.Common.Response;

namespace SE.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult FromNoContent(Result<Unit> result) =>
        result.Match<IActionResult>(_ => NoContent(), err => ToError(err.Error));

    protected IActionResult FromResult<T>(Result<T> result) =>
        result.Match<IActionResult>(
            value => Ok(ApiResponse<T>.Ok(value)),
            err => ToError(err.Error)
        );

    protected IActionResult FromCreated<T>(Result<T> result, Func<T, string> location) =>
        result.Match<IActionResult>(
            value => Created(location(value), ApiResponse<T>.Ok(value)),
            err => ToError(err.Error)
        );

    private IActionResult ToError(Error error) =>
        StatusCode(StatusCodeFor(error.Type), ApiResponse<object>.Fail(error));

    private static int StatusCodeFor(ErrorType type) =>
        type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };
}
