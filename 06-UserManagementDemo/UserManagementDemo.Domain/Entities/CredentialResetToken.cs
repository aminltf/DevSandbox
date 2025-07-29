using DevSandbox.Shared.Kernel.Base.Entities;

namespace UserManagementDemo.Domain.Entities;

public class CredentialResetToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string ResetCode { get; set; } = default!; // code or token
    public string Channel { get; set; } = default!; // "Email" or "Sms"
    public DateTime RequestedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? IpAddress { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;

    public CredentialResetToken() { }
}
