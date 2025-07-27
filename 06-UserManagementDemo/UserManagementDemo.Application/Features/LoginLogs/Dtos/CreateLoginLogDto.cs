namespace UserManagementDemo.Application.Features.LoginLogs.Dtos;

public class CreateLoginLogDto
{
    public Guid UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public string IpAddress { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public bool IsSuccess { get; set; }
    public string? FailureReason { get; set; }
}
