namespace UserManagementDemo.Application.Features.Users.Dtos;

public class GetUserByIdResultDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Status { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
