using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces;

public interface ILoginLogRepository
{
    IQueryable<LoginLog> AsQueryable(bool asNoTracking = false);
    Task<IReadOnlyList<LoginLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoginLog>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default);

    Task<LoginLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LoginLog?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LoginLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LoginLog>> GetByUserIdAsNoTrackingAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<PageResponse<LoginLog>> GetPagedAsync(
        PageRequest pagination,
        SearchRequest search,
        SortOptions sortOptions,
        CancellationToken cancellationToken = default);

    Task AddAsync(LoginLog log, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(Guid? userId = null, CancellationToken cancellationToken = default);
}
