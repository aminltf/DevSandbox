using AutoMapper;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using MediatR;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Features.Users.Dtos;

namespace UserManagementDemo.Application.Features.Users.Queries;

public record GetUsersQuery : IRequest<PageResponse<UserListDto>>
{
    public PageRequest Page { get; set; } = new();
    public SearchRequest Search { get; set; } = new();
    public SortOptions Sort { get; set; } = new();
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PageResponse<UserListDto>>
{
    private readonly IIdentityUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IIdentityUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PageResponse<UserListDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _uow.User.GetPagedAsync(
            request.Page,
            request.Search,
            request.Sort,
            cancellationToken);

        var items = _mapper.Map<List<UserListDto>>(users.Items);

        return new PageResponse<UserListDto>
        {
            Items = items,
            TotalCount = users.TotalCount,
            PageNumber = users.PageNumber,
            PageSize = users.PageSize,
        };
    }
}
