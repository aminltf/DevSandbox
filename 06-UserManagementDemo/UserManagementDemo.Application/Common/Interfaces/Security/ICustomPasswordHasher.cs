using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces.Security;

public interface ICustomPasswordHasher
{
    string HashPassword(ApplicationUser user, string password);
    PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
}
