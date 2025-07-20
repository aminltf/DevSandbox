using DevSandbox.Shared.Kernel.Abstractions.Repositories;
using DevSandbox.Shared.Kernel.Base.Entities;

namespace CleanArchitectureDemo.Application.Common.Interfaces.Repositories;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IGenericRepository<T> Repository<T>() where T : BaseSoftDeletableEntity;
    IProductRepository Product { get; }
}
