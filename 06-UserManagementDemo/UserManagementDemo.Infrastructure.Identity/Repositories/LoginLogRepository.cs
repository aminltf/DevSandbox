using DevSandbox.Shared.Kernel.Extensions;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class LoginLogRepository : ILoginLogRepository
{
    private readonly IdentityContext _context;

    public LoginLogRepository(IdentityContext context) => _context = context;

    public IQueryable<LoginLog> AsQueryable(bool asNoTracking = false)
    {
        var query = _context.LoginLogs.Include(l => l.User);
        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<IReadOnlyList<LoginLog>> GetAllAsync(CancellationToken cancellationToken = default)
        => await AsQueryable().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<LoginLog>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default)
        => await AsQueryable(asNoTracking: true).ToListAsync(cancellationToken);

    public async Task<LoginLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.LoginLogs.Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<LoginLog?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.LoginLogs.Include(l => l.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<IReadOnlyList<LoginLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.LoginLogs.Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LoginTime)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<LoginLog>> GetByUserIdAsNoTrackingAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.LoginLogs.Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LoginTime)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<PageResponse<LoginLog>> GetPagedAsync(
        PageRequest pagination,
        SearchRequest search,
        SortOptions sortOptions,
        CancellationToken cancellationToken = default)
    {
        var query = _context.LoginLogs.AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(search?.SearchTerm))
        {
            var term = $"%{search.SearchTerm.Trim()}%";
            query = query.Where(x =>
                EF.Functions.Like(x.IpAddress, term) ||
                EF.Functions.Like(x.LoginTime.ToString(), term)
            );
        }

        // Sorting
        query = query.ApplySorting(sortOptions);

        // Count
        var totalCount = await query.CountAsync(cancellationToken);

        // Paging + Projection
        var items = await query
            .ApplyPaging(pagination)
            .ToListAsync(cancellationToken);

        return new PageResponse<LoginLog>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task AddAsync(LoginLog log, CancellationToken cancellationToken = default)
    {
        await _context.LoginLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.LoginLogs.AsQueryable();
        if (userId.HasValue)
            query = query.Where(l => l.UserId == userId.Value);

        return await query.CountAsync(cancellationToken);
    }
}
