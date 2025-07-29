using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Features.PasswordResetRequests.Commands;

public record ResetPasswordCommand(ResetPasswordDto ResetPassword) : IRequest<ResetPasswordResultDto>;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityUnitOfWork _unitOfWork;

    public ResetPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IIdentityUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResetPasswordResultDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Find active reset request
        var reset = await _unitOfWork.CredentialResetToken.GetByCodeAsync(request.ResetPassword.ResetCode, cancellationToken);

        if (reset == null || reset.IsUsed || reset.ExpiresAt < DateTime.UtcNow)
        {
            return new ResetPasswordResultDto
            {
                Success = false,
                Message = "Invalid or expired reset code."
            };
        }

        var user = await _userManager.FindByIdAsync(reset.UserId.ToString());
        if (user == null || user.IsDeleted)
        {
            return new ResetPasswordResultDto
            {
                Success = false,
                Message = "Invalid or expired reset code."
            };
        }

        // Change password with UserManager (bypass old password)
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.ResetPassword.NewPassword);

        if (!result.Succeeded)
        {
            var errorMsg = result.Errors.FirstOrDefault()?.Description ?? "Password reset failed.";
            return new ResetPasswordResultDto { Success = false, Message = errorMsg };
        }

        // Invalidate the reset request
        reset.IsUsed = true;
        reset.UsedAt = DateTime.UtcNow;
        await _unitOfWork.CredentialResetToken.UpdateAsync(reset, cancellationToken);

        // Optionally: force password change on next login? (depends on policy)
        user.IsPasswordChangeRequired = false;
        user.PasswordChangedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResetPasswordResultDto
        {
            Success = true,
            Message = "Password has been reset successfully."
        };
    }
}
