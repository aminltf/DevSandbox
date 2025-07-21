using AutoMapper;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Queries.GetAll;

public class GetProductsQuery : IRequest<PageResponse<ProductListDto>>
{
    public PageRequest Page { get; set; } = new();
    public SearchRequest Search { get; set; } = new();
    public SortOptions Sort { get; set; } = new();
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PageResponse<ProductListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PageResponse<ProductListDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Product.GetProductsAsync(
            request.Page,
            request.Search,
            request.Sort,
            cancellationToken);

        var items = _mapper.Map<List<ProductListDto>>(entity.Items);

        return new PageResponse<ProductListDto>
        {
            Items = items,
            TotalCount = entity.TotalCount,
            PageNumber = entity.PageNumber,
            PageSize = entity.PageSize
        };
    }
}
