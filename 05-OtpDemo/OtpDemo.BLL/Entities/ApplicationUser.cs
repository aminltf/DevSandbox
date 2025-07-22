using Microsoft.AspNetCore.Identity;

namespace OtpDemo.BLL.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    // Custom properties
    public DateTime? LastLoginAt { get; set; }
    // No password needed!
}
