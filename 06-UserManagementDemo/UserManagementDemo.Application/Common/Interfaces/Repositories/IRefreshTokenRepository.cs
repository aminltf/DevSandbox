using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task RevokeAllActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddRefreshTokenAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
