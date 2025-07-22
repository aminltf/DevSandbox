using AuthDemo.Domain.Entities;

namespace AuthDemo.Application.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
}
