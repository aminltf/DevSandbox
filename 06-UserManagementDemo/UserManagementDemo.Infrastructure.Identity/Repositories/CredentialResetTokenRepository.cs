using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class CredentialResetTokenRepository : ICredentialResetTokenRepository
{
    private readonly IdentityContext _context;

    public CredentialResetTokenRepository(IdentityContext context)
    {
        _context = context;
    }

    public async Task<CredentialResetToken?> GetValidRequestByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.CredentialResetTokens
            .FirstOrDefaultAsync(r =>
                r.ResetCode == code &&
                !r.IsUsed &&
                r.ExpiresAt > now,
                cancellationToken);
    }

    public async Task AddAsync(CredentialResetToken request, CancellationToken cancellationToken = default)
    {
        await _context.CredentialResetTokens.AddAsync(request, cancellationToken);
    }
    
    public Task UpdateAsync(CredentialResetToken entity, CancellationToken cancellationToken = default)
    {
        _context.CredentialResetTokens.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<CredentialResetToken?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        // Only unused and not expired
        return await _context.CredentialResetTokens
            .FirstOrDefaultAsync(r =>
                r.ResetCode == code &&
                !r.IsUsed &&
                r.ExpiresAt > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task InvalidateAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var all = await _context.CredentialResetTokens
            .Where(r => r.UserId == userId && !r.IsUsed && r.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var item in all)
        {
            item.IsUsed = true;
            item.UsedAt = DateTime.UtcNow;
        }
    }

    public async Task<int> CountRecentRequestsAsync(Guid userId, DateTime from, CancellationToken cancellationToken = default)
    {
        return await _context.CredentialResetTokens
            .CountAsync(r => r.UserId == userId && r.RequestedAt >= from, cancellationToken);
    }
}
