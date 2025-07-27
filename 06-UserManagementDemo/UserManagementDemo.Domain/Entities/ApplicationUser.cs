using DevSandbox.Shared.Kernel.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public UserStatus Status { get; set; } = UserStatus.Active;
    public DateTime? PasswordChangedAt { get; set; }
    public DateTime? LockedAt { get; set; }
    public string? LockReason { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public ICollection<LoginLog> LoginLogs { get; set; } = new List<LoginLog>();

    public ApplicationUser() { }

    // Domain methods for business logic
    public void Activate() => Status = UserStatus.Active;
    public void Deactivate() => Status = UserStatus.Inactive;

    public void Lock(string reason)
    {
        Status = UserStatus.Locked;
        LockedAt = DateTime.UtcNow;
        LockReason = reason;
    }

    public void Unlock()
    {
        Status = UserStatus.Active;
        LockedAt = null;
        LockReason = null;
        AccessFailedCount = 0;
    }

    public void SoftDelete() => Status = UserStatus.Deleted;
    public void Restore() => Status = UserStatus.Active;

    public void ChangePassword(DateTime changedAt)
    {
        PasswordChangedAt = changedAt;
        AccessFailedCount = 0;
    }

    public void IncreaseFailedCount()
    {
        AccessFailedCount++;
        if (AccessFailedCount >= 5)
            Lock("Account locked due to 5 failed login attempts");
    }

    public void ResetFailedCount() => AccessFailedCount = 0;

    public void SetRole(UserRole role) => Role = role;
}
