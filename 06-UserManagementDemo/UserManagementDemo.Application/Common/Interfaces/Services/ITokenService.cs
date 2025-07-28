using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces.Services;

public interface ITokenService
{
    string GenerateJwtToken(ApplicationUser user);
    RefreshToken GenerateRefreshToken();
    int TokenLifetimeSeconds { get; }
}
