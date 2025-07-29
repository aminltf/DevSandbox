using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Infrastructure.Identity.Contexts;

namespace UserManagementDemo.Infrastructure.Identity.Repositories;

public class IdentityUnitOfWork : IIdentityUnitOfWork
{
    private readonly IdentityContext _context;

    public IdentityUnitOfWork(
        IdentityContext context,
        IUserRepository user,
        ILoginLogRepository loginLog,
        IRefreshTokenRepository refreshToken,
        ICredentialResetTokenRepository credentialResetToken)
    {
        _context = context;
        User = user;
        LoginLog = loginLog;
        RefreshToken = refreshToken;
        CredentialResetToken = credentialResetToken;
    }

    public IUserRepository User { get; }
    public ILoginLogRepository LoginLog { get; }
    public IRefreshTokenRepository RefreshToken { get; }
    public ICredentialResetTokenRepository CredentialResetToken { get; }

    public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
