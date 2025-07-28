using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Infrastructure.Identity.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.Property(r => r.Token)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(r => r.CreatedByIp)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(r => r.RevokedByIp)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(r => r.ReplacedByToken)
            .HasMaxLength(512)
            .IsRequired(false);

        builder.HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId);
    }
}
