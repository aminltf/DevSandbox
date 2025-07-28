using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;

namespace UserManagementDemo.Application.Features.RefreshTokens.Commands;

public record RevokeAllRefreshTokensCommand(Guid UserId) : IRequest<bool>;

public class RevokeAllRefreshTokensCommandHandler : IRequestHandler<RevokeAllRefreshTokensCommand, bool>
{
    private readonly IIdentityUnitOfWork _unitOfWork;

    public RevokeAllRefreshTokensCommandHandler(IIdentityUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RevokeAllRefreshTokensCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.RefreshToken.RevokeAllActiveRefreshTokensAsync(request.UserId, cancellationToken);

        // SaveChanges
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }
}
