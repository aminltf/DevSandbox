using OtpDemo.BLL.Entities;

namespace OtpDemo.BLL.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}
