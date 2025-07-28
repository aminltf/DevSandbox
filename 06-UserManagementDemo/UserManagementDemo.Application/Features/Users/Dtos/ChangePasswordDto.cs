namespace UserManagementDemo.Application.Features.Users.Dtos;

public class ChangePasswordDto
{
    public string UserName { get; set; } = default!;
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
