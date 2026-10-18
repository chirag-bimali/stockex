namespace SE.Application.Common.Response;

public sealed record Error(
    string Code,
    string Description,
    ErrorType Type,
    IDictionary<string, string[]>? Details = null)
{
    // "no error" placeholder
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    // Shortcuts so you don't write the type every time
    public static Error Validation(string code, string description, IDictionary<string, string[]>? details = null)
        => new(code, description, ErrorType.Validation, details);

    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

    public static Error UnauthorizedLogin(string code, string description, IDictionary<string, string[]> Details) => new(code, description, ErrorType.Unauthorized, Details);
    public static Error Unauthorized(string code, string description) => new(code, description, ErrorType.Unauthorized);
    public static Error Forbidden(string code, string description) => new(code, description, ErrorType.Forbidden);
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);
    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);
}