using DevSandbox.Shared.Kernel.Extensions;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using Microsoft.EntityFrameworkCore;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class UserRepository : IUserRepository // ToDo: Warning for SaveChanges
{
    private readonly IdentityContext _context;

    public UserRepository(IdentityContext context) => _context = context;

    public IQueryable<ApplicationUser> AsQueryable(bool includeDeleted = false, bool asNoTracking = false)
    {
        var query = includeDeleted
            ? _context.Users
            : _context.Users.Where(u => u.Status != UserStatus.Deleted);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
        => await AsQueryable(includeDeleted: false).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ApplicationUser>> GetAllDeletedAsync(CancellationToken cancellationToken = default)
        => await AsQueryable(includeDeleted: true)
                .Where(u => u.Status == UserStatus.Deleted)
                .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ApplicationUser>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default)
        => await AsQueryable(includeDeleted: false, asNoTracking: true).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ApplicationUser>> GetAllDeletedAsNoTrackingAsync(CancellationToken cancellationToken = default)
        => await AsQueryable(includeDeleted: true, asNoTracking: true)
                .Where(u => u.Status == UserStatus.Deleted)
                .ToListAsync(cancellationToken);

    public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<ApplicationUser?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<ApplicationUser?> GetByUserNameAsNoTrackingAsync(string userName, CancellationToken cancellationToken = default)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(u => u.Id == id && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(u => u.UserName == userName && u.Status != UserStatus.Deleted, cancellationToken);

    public async Task<IReadOnlyList<ApplicationUser>> GetByStatusAsync(UserStatus status, CancellationToken cancellationToken = default)
        => await _context.Users.Where(u => u.Status == status).ToListAsync(cancellationToken);

    public async Task<PageResponse<ApplicationUser>> GetPagedAsync(
        PageRequest pagination,
        SearchRequest search,
        SortOptions sortOptions,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsQueryable();

        query = query.Where(u => u.Status != UserStatus.Deleted);

        // Search
        if (!string.IsNullOrWhiteSpace(search?.SearchTerm))
        {
            var term = $"%{search.SearchTerm.Trim()}%";
            query = query.Where(x =>
                EF.Functions.Like(x.UserName, term) ||
                EF.Functions.Like(x.Status.ToString(), term) ||
                EF.Functions.Like(x.Role.ToString(), term)
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

        return new PageResponse<ApplicationUser>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize
        };
    }

    public async Task AddAsync(ApplicationUser entity, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ApplicationUser entity, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RestoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user == null)
            return false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ActivateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeactivateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ChangePasswordAsync(
        ApplicationUser user,
        string newPasswordHash,
        DateTime passwordChangedAt,
        CancellationToken cancellationToken = default)
    {
        user.PasswordHash = newPasswordHash;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
