namespace UserManagementDemo.Application.Features.Users.Dtos;

public class ResetPasswordDto
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; } = null!;
}
