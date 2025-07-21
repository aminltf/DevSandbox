using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using DevSandbox.Shared.Kernel.CQRS.Commands;
using MediatR;

namespace CleanArchitectureDemo.Application.Features.Products.Commands.Restore;

public record RestoreProductCommand(Guid Id) : RestoreCommand<Guid, bool>(Id), IRequest<bool>;

public class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RestoreProductCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.Product.RestoreAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
