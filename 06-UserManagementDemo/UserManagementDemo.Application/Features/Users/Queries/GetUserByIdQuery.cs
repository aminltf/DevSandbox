using AutoMapper;
using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid UserId, bool AsNoTracking = true) : IRequest<GetUserByIdResultDto?>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResultDto?>
{
    private readonly IIdentityUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IIdentityUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<GetUserByIdResultDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        ApplicationUser? user;
        if (request.AsNoTracking)
            user = await _uow.User.GetByIdAsNoTrackingAsync(request.UserId, cancellationToken);
        else
            user = await _uow.User.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return null;

        return _mapper.Map<GetUserByIdResultDto>(user);
    }
}
