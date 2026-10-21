using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Domain.Entities;

namespace SE.Infrastructure.Persistence.Configurations;

class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
  public void Configure(EntityTypeBuilder<RefreshToken> builder)
  {
    builder.HasKey(rt => rt.Id);

    builder.Property(rt => rt.TokenHash)
        .IsRequired()
        .HasMaxLength(512);

    builder.Property(rt => rt.ExpiresAt)
        .IsRequired();

    builder.Property(rt => rt.CreatedAt)
        .IsRequired();

    builder.Property(rt => rt.RevokedAt)
        .IsRequired(false);

    builder.HasOne(rt => rt.User)
        .WithMany(u => u.RefreshTokens)
        .HasForeignKey(rt => rt.UserId);
  }
}

