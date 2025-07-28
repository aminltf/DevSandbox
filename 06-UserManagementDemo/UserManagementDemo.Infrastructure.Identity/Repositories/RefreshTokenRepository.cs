using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityContext _context;

    public RefreshTokenRepository(IdentityContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(token, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAllActiveRefreshTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.Revoked == null && t.Expires > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRefreshTokenAsync(Guid userId, RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        refreshToken.UserId = userId;
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
