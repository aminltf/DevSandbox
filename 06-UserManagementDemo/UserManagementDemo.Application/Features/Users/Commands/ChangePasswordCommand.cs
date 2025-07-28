using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Security;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record ChangePasswordCommand(ChangePasswordDto ChangePassword) : IRequest<ChangePasswordResultDto>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResultDto>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly ICustomPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public ChangePasswordCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ICustomPasswordHasher hasher,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<ChangePasswordResultDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByUserNameAsync(request.ChangePassword.UserName, cancellationToken);
        if (user == null || user.IsDeleted)
            return new ChangePasswordResultDto { Success = false, Message = "User not found." };

        var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.ChangePassword.OldPassword);
        if (verify != PasswordVerificationResult.Success)
            return new ChangePasswordResultDto { Success = false, Message = "Current password is incorrect." };

        if (request.ChangePassword.OldPassword == request.ChangePassword.NewPassword)
            return new ChangePasswordResultDto { Success = false, Message = "New password must be different from old password." };

        user.PasswordHash = _hasher.HashPassword(user, request.ChangePassword.NewPassword);
        user.IsPasswordChangeRequired = false;
        user.PasswordChangedAt = DateTime.UtcNow;

        await _unitOfWork.RefreshToken.RevokeAllActiveRefreshTokensAsync(user.Id, cancellationToken);

        var refreshToken = _tokenService.GenerateRefreshToken();
        await _unitOfWork.RefreshToken.AddRefreshTokenAsync(user.Id, refreshToken, cancellationToken);

        // SaveChanges
        await _unitOfWork.CommitAsync(cancellationToken);

        var jwt = _tokenService.GenerateJwtToken(user);

        return new ChangePasswordResultDto
        {
            Success = true,
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Message = "Password changed successfully."
        };
    }
}
