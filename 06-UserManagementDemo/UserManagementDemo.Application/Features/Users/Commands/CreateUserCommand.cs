using MediatR;
using AutoMapper;
using DevSandbox.Shared.Kernel.Exceptions;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Security;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Domain.Enums;

namespace UserManagementDemo.Application.Features.Users.Commands;

public record CreateUserCommand(CreateUserDto CreateUser) : IRequest<CreateUserResultDto>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResultDto>
{
    private readonly IIdentityUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ICustomPasswordHasher _hasher;
    private readonly ICurrentUserService _currentUser;

    public CreateUserCommandHandler(
        IIdentityUnitOfWork uow,
        IMapper mapper,
        ICustomPasswordHasher hasher,
        ICurrentUserService currentUser)
    {
        _uow = uow;
        _mapper = mapper;
        _hasher = hasher;
        _currentUser = currentUser;
    }

    public async Task<CreateUserResultDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _uow.User.ExistsByUserNameAsync(request.CreateUser.UserName, cancellationToken))
            throw new ConflictException("Username is already taken.");

        var user = _mapper.Map<ApplicationUser>(request.CreateUser);

        user.Id = Guid.NewGuid();
        user.PasswordHash = _hasher.HashPassword(user, request.CreateUser.Password);
        user.Status = UserStatus.Active;
        user.CreatedAt = DateTime.UtcNow;
        user.CreatedBy = _currentUser.Session.UserName;
        user.Role = (UserRole)request.CreateUser.Role;
        user.IsPasswordChangeRequired = true;
        user.IsDeleted = false;

        await _uow.User.AddAsync(user, cancellationToken);

        // Commit
        await _uow.CommitAsync(cancellationToken);

        return new CreateUserResultDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? request.CreateUser.UserName,
            Role = user.Role.ToString()
        };
    }
}
