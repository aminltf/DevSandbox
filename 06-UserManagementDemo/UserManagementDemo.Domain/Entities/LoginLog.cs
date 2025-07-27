using DevSandbox.Shared.Kernel.Base.Entities;

namespace UserManagementDemo.Domain.Entities;

public class LoginLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; } // Navigation property

    public DateTime LoginTime { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool IsSuccess { get; set; }

    public string? FailureReason { get; set; }

    public LoginLog() { }
}
