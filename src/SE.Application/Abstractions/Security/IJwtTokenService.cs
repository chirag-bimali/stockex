using SE.Domain.Entities;

namespace SE.Application.Abstractions.Security;

public interface IJwtTokenService
{
    public GeneratedToken GenerateAccessToken(User user, IEnumerable<string> roles);
}
