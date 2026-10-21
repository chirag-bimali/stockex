using Microsoft.EntityFrameworkCore;
using SE.Application.Abstractions.Security;
using SE.Domain.Entities;
using SE.Infrastructure.Persistence.Configurations;
using SE.Infrastructure.Persistence.Seeding;

public class UserSeeder(AppDbContext db, IPasswordHasher passwordHasher) : ISeeder
{
    public int Order => 10;
    public static readonly (
        Guid Id,
        string Email,
        string Password,
        string FirstName,
        string LastName,
        bool IsActive,
        bool IsEmailVerified,
        int FailedLoginAttempts,
        DateTimeOffset? LockedUntil,
        DateTimeOffset CreatedAt,
        DateTimeOffset? UpdatedAt
    )[] Users =
    [
        (
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            "admin@admin.com",
            "Admin@1234",
            "admin",
            "admin",
            true,
            false,
            0,
            null,
            DateTimeOffset.UtcNow,
            null
        ),
    ];

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var seedUserEmails = Users.Select(u => u.Email).ToList();
        // Seed users
        var existingUsers = await db
            .Users.Where(u => seedUserEmails.Contains(u.Email))
            .ToListAsync(ct);

        foreach (var user in Users)
        {
            if (!existingUsers.Any(u => u.Email == user.Email))
            {
                var newUser = new User(
                    user.Id,
                    user.Email,
                    passwordHasher.HashPassword(user.Password),
                    user.FirstName,
                    user.LastName
                );
                db.Users.Add(newUser);
            }
        }
        await db.SaveChangesAsync(ct);
    }
}
