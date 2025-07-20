using CleanArchitectureDemo.Domain.Entities;
using DevSandbox.Shared.Kernel.Abstractions.Repositories;

namespace CleanArchitectureDemo.Application.Common.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IReadOnlyList<Product>> GetActiveProductsAsync();
}
