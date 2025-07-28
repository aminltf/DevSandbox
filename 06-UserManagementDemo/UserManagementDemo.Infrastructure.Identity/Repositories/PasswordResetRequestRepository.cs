using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class PasswordResetRequestRepository : IPasswordResetRequestRepository
{
    private readonly IdentityContext _context;

    public PasswordResetRequestRepository(IdentityContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetRequest?> GetValidRequestByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.PasswordResetRequests
            .FirstOrDefaultAsync(r =>
                r.ResetCode == code &&
                !r.IsUsed &&
                r.ExpiresAt > now,
                cancellationToken);
    }

    public async Task AddAsync(PasswordResetRequest request, CancellationToken cancellationToken = default)
    {
        await _context.PasswordResetRequests.AddAsync(request, cancellationToken);
    }

    public async Task InvalidateAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var all = await _context.PasswordResetRequests
            .Where(r => r.UserId == userId && !r.IsUsed && r.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var item in all)
        {
            item.IsUsed = true;
            item.CompletedAt = DateTime.UtcNow;
        }
    }

    public async Task<int> CountRecentRequestsAsync(Guid userId, DateTime from, CancellationToken cancellationToken = default)
    {
        return await _context.PasswordResetRequests
            .CountAsync(r => r.UserId == userId && r.RequestedAt >= from, cancellationToken);
    }
}
