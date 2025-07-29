using MediatR;
using Microsoft.AspNetCore.Identity;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record UpdateUserCommand(UpdateUserDto UpdateUser) : IRequest<UpdateUserResultDto>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UpdateUserResultDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Find user by Id
        var user = await _userManager.FindByIdAsync(request.UpdateUser.Id.ToString());
        if (user == null || user.IsDeleted)
            return new UpdateUserResultDto { Success = false, Message = "User not found." };

        // Update allowed fields
        if (!string.IsNullOrWhiteSpace(request.UpdateUser.FirstName))
            user.FirstName = request.UpdateUser.FirstName;
        if (!string.IsNullOrWhiteSpace(request.UpdateUser.LastName))
            user.LastName = request.UpdateUser.LastName;
        if (!string.IsNullOrWhiteSpace(request.UpdateUser.Email) && user.Email != request.UpdateUser.Email)
            user.Email = request.UpdateUser.Email;
        if (request.UpdateUser.Role.HasValue)
            user.Role = (UserRole)request.UpdateUser.Role.Value;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new UpdateUserResultDto { Success = false, Message = string.Join("; ", errors) };
        }

        return new UpdateUserResultDto { Success = true, Message = "User updated successfully." };
    }
}
