using Microsoft.AspNetCore.Identity;

namespace AuthDemo.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    // Custom properties
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? FullName { get; set; }
    // ... any other domain properties
}
