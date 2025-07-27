namespace UserManagementDemo.Application.Features.Users.Dtos;

public class UserListDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public int Role { get; set; }
    public string RoleName { get; set; } = null!;
    public int Status { get; set; }
    public string StatusName { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
