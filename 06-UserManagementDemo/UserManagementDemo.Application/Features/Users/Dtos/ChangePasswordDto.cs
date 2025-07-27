namespace UserManagementDemo.Application.Features.Users.Dtos;

public class ChangePasswordDto
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
