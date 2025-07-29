using MediatR;
using AutoMapper;
using DevSandbox.Shared.Kernel.Exceptions;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record CreateUserCommand(CreateUserDto CreateUser) : IRequest<CreateUserResultDto>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResultDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _service;

    public CreateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ICurrentUserService service)
    {
        _userManager = userManager;
        _mapper = mapper;
        _service = service;
    }


    public async Task<CreateUserResultDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate username
        var existing = await _userManager.FindByNameAsync(request.CreateUser.UserName);
        if (existing != null)
            throw new ConflictException("Username is already taken.");

        // Map DTO to entity (excluding password)
        var user = _mapper.Map<ApplicationUser>(request.CreateUser);

        user.Id = Guid.NewGuid();
        user.Status = UserStatus.Active;
        user.CreatedAt = DateTime.UtcNow;
        user.CreatedBy = _service.Session.UserName;
        user.Role = (UserRole)request.CreateUser.Role;
        user.IsPasswordChangeRequired = true;
        user.IsDeleted = false;

        // Create user (UserManager handles password hashing)
        var result = await _userManager.CreateAsync(user, request.CreateUser.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description).ToList());

        // Optionally add role (if using Identity roles)
        // await _userManager.AddToRoleAsync(user, user.Role.ToString());

        // Additional logic (e.g., send email verification) can be placed here

        return new CreateUserResultDto
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Role = user.Role.ToString()
        };
    }
}
