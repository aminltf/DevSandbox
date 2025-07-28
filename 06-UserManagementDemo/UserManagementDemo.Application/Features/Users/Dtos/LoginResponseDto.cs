namespace UserManagementDemo.Application.Features.Users.Dtos;

public class LoginResponseDto
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string UserName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public bool IsPasswordChangeRequired { get; set; }
    public int ExpiresIn { get; set; }
    public string? Message { get; set; }
}
