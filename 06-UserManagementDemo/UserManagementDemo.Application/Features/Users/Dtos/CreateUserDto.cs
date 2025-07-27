namespace UserManagementDemo.Application.Features.Users.Dtos;

public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Role { get; set; }
}
