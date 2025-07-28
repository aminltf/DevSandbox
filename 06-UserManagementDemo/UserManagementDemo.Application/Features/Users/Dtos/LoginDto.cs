namespace UserManagementDemo.Application.Features.Users.Dtos;

public class LoginDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
