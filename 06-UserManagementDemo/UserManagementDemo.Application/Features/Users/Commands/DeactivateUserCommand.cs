using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record DeactivateUserCommand(Guid UserId) : IRequest<UpdateUserResultDto>;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, UpdateUserResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeactivateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UpdateUserResultDto> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null || user.IsDeleted)
            return new UpdateUserResultDto { Success = false, Message = "User not found." };

        if (user.Status == UserStatus.Inactive)
            return new UpdateUserResultDto { Success = false, Message = "User is already inactive." };

        user.Status = UserStatus.Inactive;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return new UpdateUserResultDto { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.Description)) };

        return new UpdateUserResultDto { Success = true, Message = "User deactivated successfully." };
    }
}
