using DevSandbox.Shared.Kernel.Base.Entities;

namespace UserManagementDemo.Domain.Entities;

public class LoginLog : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public string? IpAddress { get; set; }

    public ApplicationUser User { get; set; } = default!;

    public LoginLog() { }
}
