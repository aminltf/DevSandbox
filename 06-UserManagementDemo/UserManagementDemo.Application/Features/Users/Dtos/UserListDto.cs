namespace UserManagementDemo.Application.Features.Users.Dtos;

public class UserListDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
}
