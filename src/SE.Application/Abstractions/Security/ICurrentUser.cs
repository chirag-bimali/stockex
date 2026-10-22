namespace Application.Abstractions.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    Guid? Id { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
    bool IsInRole(string role);
}
