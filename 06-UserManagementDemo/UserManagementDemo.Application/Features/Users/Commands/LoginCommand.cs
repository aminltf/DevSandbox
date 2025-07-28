using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Security;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record LoginCommand(LoginDto Login) : IRequest<LoginResponseDto>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly ICustomPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IIdentityUnitOfWork unitOfWork,
        ICustomPasswordHasher hasher,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.User.GetByUserNameAsync(request.Login.UserName, cancellationToken);

        if (user == null || user.IsDeleted || user.Status != UserStatus.Active)
        {
            return new LoginResponseDto { Message = "Invalid username or password." };
        }

        var passwordResult = _hasher.VerifyHashedPassword(user, user.PasswordHash!, request.Login.Password);
        if (passwordResult != PasswordVerificationResult.Success)
        {
            return new LoginResponseDto { Message = "Invalid username or password." };
        }

        if (user.IsPasswordChangeRequired)
        {
            return new LoginResponseDto
            {
                IsPasswordChangeRequired = true,
                UserName = user.UserName!,
                Role = user.Role.ToString(),
                Message = "You must change your password before continuing."
            };
        }

        await _unitOfWork.RefreshToken.RevokeAllActiveRefreshTokensAsync(user.Id, cancellationToken);

        var jwtToken = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _unitOfWork.RefreshToken.AddRefreshTokenAsync(user.Id, refreshToken, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            UserName = user.UserName!,
            Role = user.Role.ToString(),
            IsPasswordChangeRequired = false,
            ExpiresIn = _tokenService.TokenLifetimeSeconds
        };
    }
}
