namespace SE.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public bool IsActive { get; private set; } = true;

    public bool IsEmailVerified { get; private set; }

    public int FailedLoginAttempts { get; private set; }

    public DateTimeOffset? LockedUntil { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; private set; }

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    private User() { } // EF Core

    public User(Guid id, string email, string passwordHash, string firstName, string lastName)
    {
        Id = id;
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public User(string email, string passwordHash, string firstName, string lastName)
    {
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedAt = DateTimeOffset.UtcNow;
        FailedLoginAttempts = 0;
        LockedUntil = null;
    }

    public void AssignRole(Role role)
    {
        if (UserRoles.Any(ur => ur.RoleId == role.Id))
            return; // already has this role

        UserRoles.Add(new UserRole(Id, role.Id));
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;

        if (FailedLoginAttempts >= 5)
        {
            LockedUntil = DateTimeOffset.UtcNow.AddMinutes(15);
        }
    }

    public void RecordSuccessfulLogin()
    {
        FailedLoginAttempts = 0;
        LockedUntil = null;
    }
}
