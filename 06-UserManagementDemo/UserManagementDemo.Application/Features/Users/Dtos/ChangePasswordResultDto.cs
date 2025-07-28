namespace UserManagementDemo.Application.Features.Users.Dtos;

public class ChangePasswordResultDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? Message { get; set; }
}
