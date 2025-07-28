using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.RefreshTokens.Dtos;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.RefreshTokens.Commands;

public record RefreshTokenCommand(RefreshTokenRequestDto Request) : IRequest<RefreshTokenResponseDto>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _unitOfWork.RefreshToken
            .GetByTokenAsync(request.Request.RefreshToken, cancellationToken);

        if (refreshToken == null || refreshToken.Revoked != null || refreshToken.Expires < DateTime.UtcNow)
        {
            return new RefreshTokenResponseDto
            {
                Message = "Invalid or expired refresh token."
            };
        }

        var user = await _unitOfWork.User.GetByIdAsync(refreshToken.UserId, cancellationToken);
        if (user == null || user.IsDeleted || user.Status != UserStatus.Active)
        {
            return new RefreshTokenResponseDto
            {
                Message = "User is not valid."
            };
        }

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.ReplacedByToken = null;

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        await _unitOfWork.RefreshToken.AddRefreshTokenAsync(user.Id, newRefreshToken, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var newJwt = _tokenService.GenerateJwtToken(user);

        return new RefreshTokenResponseDto
        {
            Token = newJwt,
            RefreshToken = newRefreshToken.Token,
            ExpiresIn = _tokenService.TokenLifetimeSeconds
        };
    }
}
