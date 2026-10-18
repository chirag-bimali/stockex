namespace SE.API.Common.Response;
public sealed record ApiError(
    string Code,
    string Message,
    IDictionary<string, string[]>? Details = null,
    Dictionary<int, Dictionary<string, string[]>>? RowErrors = null);