using UserManagementDemo.Application.Features.LoginLogs.Dtos;

namespace UserManagementDemo.Application.Features.Users.Dtos;

public class UserDetailsDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int Role { get; set; }
    public string RoleName { get; set; } = null!;
    public int Status { get; set; }
    public string StatusName { get; set; } = null!;
    public DateTime? PasswordChangedAt { get; set; }
    public DateTime? LockedAt { get; set; }
    public string LockReason { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int AccessFailedCount { get; set; }
    public List<LoginLogDto> LoginLogs { get; set; } = new List<LoginLogDto>();
}
