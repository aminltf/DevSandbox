using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record LogoutCommand(string RefreshToken) : IRequest<bool>;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IIdentityUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IIdentityUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Get refresh token by token string
        var token = await _unitOfWork.RefreshToken.GetByTokenAsync(request.RefreshToken, cancellationToken);

        // Token not found or already revoked
        if (token == null || token.Revoked != null)
            return false;

        // Revoke token
        token.Revoked = DateTime.UtcNow;

        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
