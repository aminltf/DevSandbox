namespace UserManagementDemo.Application.Features.Users.Dtos;

public class CreateUserDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Role { get; set; }
}
