using Microsoft.AspNetCore.Identity;

namespace OtpDemo.Core.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    // Custom properties
    public DateTime? LastLoginAt { get; set; }
    // No password needed!
}
