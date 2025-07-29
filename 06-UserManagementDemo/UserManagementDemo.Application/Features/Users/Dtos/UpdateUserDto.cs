namespace UserManagementDemo.Application.Features.Users.Dtos;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int? Role { get; set; }
    // Add more fields as needed, exclude UserName/Id from change unless super admin
}
