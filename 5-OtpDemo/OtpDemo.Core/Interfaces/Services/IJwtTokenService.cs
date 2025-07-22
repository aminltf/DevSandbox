using OtpDemo.Core.Entities;

namespace OtpDemo.Core.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}
