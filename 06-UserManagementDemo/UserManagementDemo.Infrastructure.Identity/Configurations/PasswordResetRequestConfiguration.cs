using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Infrastructure.Identity.Configurations;

public class PasswordResetRequestConfiguration : IEntityTypeConfiguration<PasswordResetRequest>
{
    public void Configure(EntityTypeBuilder<PasswordResetRequest> builder)
    {
        builder.ToTable("PasswordResetRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResetCode)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Channel)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.RequestedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.CompletedAt)
            .IsRequired(false);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.IsUsed)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(u => u.PasswordResetRequests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ResetCode).IsUnique();

        builder.HasIndex(x => new { x.UserId, x.RequestedAt });

        builder.HasIndex(x => new { x.UserId, x.IsUsed, x.ExpiresAt });
    }
}
