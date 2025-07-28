using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces.Repositories;

public interface IPasswordResetRequestRepository
{
    Task<PasswordResetRequest?> GetValidRequestByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(PasswordResetRequest request, CancellationToken cancellationToken = default);
    Task InvalidateAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountRecentRequestsAsync(Guid userId, DateTime from, CancellationToken cancellationToken = default);
}
