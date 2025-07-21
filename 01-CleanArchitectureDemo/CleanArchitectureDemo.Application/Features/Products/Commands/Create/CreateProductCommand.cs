using AutoMapper;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using CleanArchitectureDemo.Domain.Entities;
using DevSandbox.Shared.Kernel.CQRS.Commands;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Commands.Create;

// Command
public record CreateProductCommand(CreateProductDto Model) : CreateCommand<CreateProductDto, Guid>(Model), IRequest<Guid>;

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Product>(request.Model);
        await _unitOfWork.Product.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
