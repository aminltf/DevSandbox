using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record ChangePasswordCommand(ChangePasswordDto ChangePassword) : IRequest<ChangePasswordResultDto>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public ChangePasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IIdentityUnitOfWork unitOfWork,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<ChangePasswordResultDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        // Find user by username
        var user = await _userManager.FindByNameAsync(request.ChangePassword.UserName);
        if (user == null || user.IsDeleted)
            return new ChangePasswordResultDto { Success = false, Message = "User not found." };

        // Change password via Identity
        var result = await _userManager.ChangePasswordAsync(user, request.ChangePassword.OldPassword, request.ChangePassword.NewPassword);
        if (!result.Succeeded)
        {
            var msg = result.Errors.FirstOrDefault()?.Description ?? "Current password is incorrect.";
            return new ChangePasswordResultDto { Success = false, Message = msg };
        }

        // Update user fields
        user.IsPasswordChangeRequired = false;
        user.PasswordChangedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Revoke all active refresh tokens
        await _unitOfWork.RefreshToken.RevokeAllActiveRefreshTokensAsync(user.Id, cancellationToken);

        // Add new refresh token
        var refreshToken = _tokenService.GenerateRefreshToken();
        await _unitOfWork.RefreshToken.AddRefreshTokenAsync(user.Id, refreshToken, cancellationToken);

        // Commit all changes
        await _unitOfWork.CommitAsync(cancellationToken);

        // Generate new JWT token
        var jwt = _tokenService.GenerateJwtToken(user);

        // Return result
        return new ChangePasswordResultDto
        {
            Success = true,
            Token = jwt,
            RefreshToken = refreshToken.Token,
            Message = "Password changed successfully."
        };
    }
}
