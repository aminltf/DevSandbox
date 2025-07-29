using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    IQueryable<ApplicationUser> AsQueryable(bool includeDeleted = false, bool asNoTracking = false);

    Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApplicationUser>> GetAllDeletedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApplicationUser>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApplicationUser>> GetAllDeletedAsNoTrackingAsync(CancellationToken cancellationToken = default);

    Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByUserNameAsNoTrackingAsync(string userName, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> GetByUserNameOrMobileAsync(string userNameOrMobileOrEmail, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationUser>> GetByStatusAsync(UserStatus status, CancellationToken cancellationToken = default);

    Task<PageResponse<ApplicationUser>> GetPagedAsync(PageRequest pagination, SearchRequest search, SortOptions sortOptions, CancellationToken cancellationToken = default);

    Task AddAsync(ApplicationUser entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApplicationUser entity, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> RestoreAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ActivateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<bool> DeactivateAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(ApplicationUser user, string newPasswordHash, DateTime passwordChangedAt, CancellationToken cancellationToken = default);
}
