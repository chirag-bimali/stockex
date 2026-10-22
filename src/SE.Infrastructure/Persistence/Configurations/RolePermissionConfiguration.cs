using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SE.Domain.Entities;

namespace SE.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne<Role>().WithMany(r => r.RolePermissions).HasForeignKey(rp => rp.RoleId);
        builder
            .HasOne<Permission>()
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);
    }
}
