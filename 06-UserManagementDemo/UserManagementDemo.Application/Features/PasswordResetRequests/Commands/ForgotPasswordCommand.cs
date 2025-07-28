using DevSandbox.Shared.Kernel.Utilities;
using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Features.PasswordResetRequests.Commands;

public record ForgotPasswordCommand(string UserNameOrMobile, string Channel) : IRequest<ResetPasswordResultDto>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ResetPasswordResultDto>
{
    private readonly IIdentityUnitOfWork _uow;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public ForgotPasswordCommandHandler(
        IIdentityUnitOfWork uow,
        IEmailService emailService,
        ISmsService smsService)
    {
        _uow = uow;
        _emailService = emailService;
        _smsService = smsService;
    }

    public async Task<ResetPasswordResultDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.User.GetByUserNameOrMobileOrEmailAsync(request.UserNameOrMobile, cancellationToken);
        if (user == null)
        {
            return new ResetPasswordResultDto
            {
                Success = true,
                Message = "If your account exists, a code/link will be sent."
            };
        }

        // invalidate all old requests
        await _uow.PasswordResetRequest.InvalidateAllForUserAsync(user.Id, cancellationToken);

        string code = request.Channel == "Sms"
            ? CodeGenerator.GenerateNumeric(6)
            : CodeGenerator.GenerateToken(32);

        var reset = new PasswordResetRequest
        {
            UserId = user.Id,
            ResetCode = code,
            Channel = request.Channel,
            RequestedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsUsed = false,
        };
        await _uow.PasswordResetRequest.AddAsync(reset, cancellationToken);

        if (request.Channel == "Sms")
            await _smsService.SendAsync(user.PhoneNumber!, $"کد بازیابی رمز عبور: {code}");
        else
            await _emailService.SendAsync(user.Email!, "Password Reset", $"کد/لینک بازیابی رمز عبور: {code}");

        return new ResetPasswordResultDto
        {
            Success = true,
            Message = "If your account exists, a code/link will be sent."
        };
    }
}
