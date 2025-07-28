using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Security;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Infrastructure.Identity.Security;

public class CustomPasswordHasher : ICustomPasswordHasher
{
    private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

    public string HashPassword(ApplicationUser user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
}
