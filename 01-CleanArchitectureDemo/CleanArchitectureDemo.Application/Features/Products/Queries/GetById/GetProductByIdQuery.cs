using AutoMapper;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using DevSandbox.Shared.Kernel.CQRS.Queries;
using DevSandbox.Shared.Kernel.Exceptions;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Queries.GetById;

public record GetProductByIdQuery(Guid Id) : GetByIdQuery<Guid, ProductDetailDto>(Id), IRequest<ProductDetailDto>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDetailDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Product.GetByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException(nameof(Products), request.Id);

        return _mapper.Map<ProductDetailDto>(entity);
    }
}
