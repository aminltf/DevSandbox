using AutoMapper;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Application.Features.Products.Dtos;
using DevSandbox.Shared.Kernel.CQRS.Commands;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Commands.Update;

// Command
public record UpdateProductCommand(UpdateProductDto Model) : UpdateCommand<UpdateProductDto, bool>(Model), IRequest<bool>;

// Handler
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Product.GetByIdAsync(request.Model.Id, cancellationToken);

        if (entity is null) return false;

        _mapper.Map(request.Model, entity);

        await _unitOfWork.Product.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
