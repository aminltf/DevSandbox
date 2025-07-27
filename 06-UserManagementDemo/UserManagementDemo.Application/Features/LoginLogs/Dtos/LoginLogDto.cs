namespace UserManagementDemo.Application.Features.LoginLogs.Dtos;

public class LoginLogDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public DateTime LoginTime { get; set; }
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public bool IsSuccess { get; set; }
    public string? FailureReason { get; set; }
}
