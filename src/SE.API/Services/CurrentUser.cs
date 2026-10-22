using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions.Security;

namespace SE.API.Services;

public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public Guid? Id
    {
        get
        {
            var value = Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? Email => Principal?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public IReadOnlyCollection<string> Roles =>
        Principal?.FindAll("role").Select(c => c.Value).ToArray() ?? [];

    public bool IsInRole(string role) => Principal?.IsInRole(role) ?? false;
}
