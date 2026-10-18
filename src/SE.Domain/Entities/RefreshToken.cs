namespace SE.Domain.Entities;
public class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public string TokenHash { get; private set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
        = DateTimeOffset.UtcNow;

    public DateTimeOffset? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public bool IsExpired =>
        DateTimeOffset.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { } // EF Core

    public RefreshToken(
        Guid userId,
        string tokenHash,
        DateTimeOffset expiresAt)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public void Revoke()
    {
        RevokedAt = DateTimeOffset.UtcNow;
    }
}
