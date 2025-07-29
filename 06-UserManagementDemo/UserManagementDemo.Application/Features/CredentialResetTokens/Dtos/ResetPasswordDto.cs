namespace UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;

public class ResetPasswordDto
{
    public string ResetCode { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
