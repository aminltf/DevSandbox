using CleanArchitectureDemo.Domain.Entities;
using DevSandbox.Shared.Kernel.Abstractions.Repositories;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;

namespace CleanArchitectureDemo.Application.Common.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IReadOnlyList<Product>> GetActiveProductsAsync();
    Task<PageResponse<Product>> GetProductsAsync(
        PageRequest page,
        SearchRequest search,
        SortOptions sort,
        CancellationToken cancellationToken);
}
