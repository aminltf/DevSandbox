using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Infrastructure.Identity.Configurations;

public class LoginLogConfiguration : IEntityTypeConfiguration<LoginLog>
{
    public void Configure(EntityTypeBuilder<LoginLog> builder)
    {
        builder.ToTable("LoginLogs");

        builder.Property(l => l.IpAddress)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.HasOne(l => l.User)
            .WithMany(u => u.LoginLogs)
            .HasForeignKey(l => l.UserId);
    }
}
