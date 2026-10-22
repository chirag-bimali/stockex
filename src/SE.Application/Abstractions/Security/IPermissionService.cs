namespace SE.Application.Abstractions.Security;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken ct);
}
