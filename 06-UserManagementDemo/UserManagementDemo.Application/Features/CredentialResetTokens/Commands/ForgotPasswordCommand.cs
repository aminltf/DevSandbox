using DevSandbox.Shared.Kernel.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.PasswordResetRequests.Commands;

public record ForgotPasswordCommand(ForgotPasswordDto ForgotPassword) : IRequest<ForgotPasswordResultDto>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public ForgotPasswordCommandHandler(
        UserManager<ApplicationUser> userManager,
        IIdentityUnitOfWork unitOfWork,
        IEmailService emailService,
        ISmsService smsService)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _smsService = smsService;
    }

    public async Task<ForgotPasswordResultDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        // Find user by username, email or mobile
        var user = await _unitOfWork.User.GetByUserNameOrMobileAsync(
            request.ForgotPassword.UserNameOrMobile, cancellationToken);

        // Return always true for security (don't disclose user existence)
        if (user == null || user.IsDeleted || user.Status != UserStatus.Active)
        {
            return new ForgotPasswordResultDto
            {
                Success = true,
                Message = "If your account exists, a code/link will be sent."
            };
        }

        // Invalidate previous requests
        await _unitOfWork.CredentialResetToken.InvalidateAllForUserAsync(user.Id, cancellationToken);

        // Generate code/token
        string code = request.ForgotPassword.Channel == "Sms"
            ? CodeGenerator.GenerateNumeric(6)    // e.g. "734921"
            : CodeGenerator.GenerateToken(32);    // e.g. Guid.NewGuid().ToString("N")

        // Add to DB
        var reset = new CredentialResetToken
        {
            UserId = user.Id,
            ResetCode = code,
            Channel = request.ForgotPassword.Channel,
            RequestedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsUsed = false
        };
        await _unitOfWork.CredentialResetToken.AddAsync(reset, cancellationToken);

        // Send code
        if (request.ForgotPassword.Channel == "Sms")
            await _smsService.SendAsync(user.PhoneNumber!, $"Password reset code: {code}");
        else
            await _emailService.SendAsync(user.Email!, "Password Reset", $"Your reset code: {code}");

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ForgotPasswordResultDto
        {
            Success = true,
            Message = "If your account exists, a code/link will be sent."
        };
    }
}
