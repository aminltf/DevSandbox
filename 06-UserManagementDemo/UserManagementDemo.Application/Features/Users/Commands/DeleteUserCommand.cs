using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest<OperationResultDto>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUser;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<OperationResultDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Hard Delete: فقط برای Admin ارشد
        if (!Enum.TryParse<UserRole>(_currentUser.Session.Role, out var currentRole) || currentRole != UserRole.Admin)
        {
            return new OperationResultDto { Success = false, Message = "Only super admins can delete users permanently." };
        }

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            return new OperationResultDto { Success = false, Message = "User not found." };

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return new OperationResultDto { Success = false, Message = string.Join("; ", result.Errors.Select(e => e.Description)) };

        return new OperationResultDto { Success = true, Message = "User deleted permanently." };
    }
}
