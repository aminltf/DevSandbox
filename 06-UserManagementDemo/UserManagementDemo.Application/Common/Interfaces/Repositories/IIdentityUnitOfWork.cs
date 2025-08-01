﻿namespace UserManagementDemo.Application.Common.Interfaces.Repositories;

public interface IIdentityUnitOfWork : IAsyncDisposable
{
    IUserRepository User { get; }
    IRefreshTokenRepository RefreshToken { get; }
    ILoginLogRepository LoginLog { get; }
    ICredentialResetTokenRepository CredentialResetToken { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
