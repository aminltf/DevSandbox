namespace UserManagementDemo.Application.Features.Users.Dtos;

public class CreateUserResultDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Role { get; set; } = default!;
}
