using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Common.Interfaces.Repositories;

public interface ICredentialResetTokenRepository
{
    Task<CredentialResetToken?> GetValidRequestByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(CredentialResetToken request, CancellationToken cancellationToken = default);
    Task UpdateAsync(CredentialResetToken entity, CancellationToken cancellationToken = default);
    Task<CredentialResetToken?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task InvalidateAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> CountRecentRequestsAsync(Guid userId, DateTime from, CancellationToken cancellationToken = default);
}
