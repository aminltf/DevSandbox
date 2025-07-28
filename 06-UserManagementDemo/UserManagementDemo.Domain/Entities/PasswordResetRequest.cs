using DevSandbox.Shared.Kernel.Base.Entities;

namespace UserManagementDemo.Domain.Entities;

public class PasswordResetRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public string ResetCode { get; set; } = default!;
    public string Channel { get; set; } = default!;
    public DateTime RequestedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? IpAddress { get; set; }
    public bool IsUsed { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;

    public PasswordResetRequest()
    {
        
    }
}
