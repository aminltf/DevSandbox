namespace UserManagementDemo.Application.Common.Models;

public class SessionInfo
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName => $"{FirstName} {LastName}".Trim();
    public string? Role { get; set; }
    public string? IpAddress { get; set; }
    public bool IsAuthenticated { get; set; }
}
