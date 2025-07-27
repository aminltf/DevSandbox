namespace UserManagementDemo.Application.Features.Users.Dtos;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public int? Role { get; set; }
    public int? Status { get; set; }
}
