using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record RestoreUserCommand(Guid UserId) : IRequest<OperationResultDto>;

public class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, OperationResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RestoreUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OperationResultDto> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null || !user.IsDeleted)
            return new OperationResultDto { Success = false, Message = "User not found or not deleted." };

        user.IsDeleted = false;
        user.Status = UserStatus.Inactive;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return new OperationResultDto { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.Description)) };

        return new OperationResultDto { Success = true, Message = "User restored successfully." };
    }
}
