using DevSandbox.Shared.Kernel.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }

    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Active;
    public bool IsPasswordChangeRequired { get; set; } = false;
    public DateTime PasswordChangedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<LoginLog> LoginLogs { get; set; } = new List<LoginLog>();

    public ApplicationUser()
    {

    }
}
