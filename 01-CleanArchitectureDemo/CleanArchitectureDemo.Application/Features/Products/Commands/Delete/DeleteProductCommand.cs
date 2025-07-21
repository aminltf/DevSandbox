using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using DevSandbox.Shared.Kernel.CQRS.Commands;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Commands.Delete;

// Command
public record DeleteProductCommand(Guid Id) : DeleteCommand<Guid, Unit>(Id), IRequest<Unit>;

// Handler
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Product.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
