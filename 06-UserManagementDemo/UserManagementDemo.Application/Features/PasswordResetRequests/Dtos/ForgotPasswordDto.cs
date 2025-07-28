namespace UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;

public class ForgotPasswordDto
{
    public string UserNameOrMobile { get; set; } = default!;
    public string Channel { get; set; } = default!; // "Email" or "Sms"
}
