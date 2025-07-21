using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using DevSandbox.Shared.Kernel.CQRS.Commands;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Commands.Delete;

public record SoftDeleteProductCommand(Guid Id) : DeleteCommand<Guid, bool>(Id), IRequest<bool>;

public class SoftDeleteProductCommandHandler : IRequestHandler<SoftDeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public SoftDeleteProductCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(SoftDeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Product.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null) return false;

        await _unitOfWork.Product.SoftDeleteAsync(entity.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
