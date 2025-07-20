using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Domain.Enums;
using CleanArchitectureDemo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationContext context) : base(context) { }

    public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()
    {
        return await _context.Products
            .Where(p => p.Status == ProductStatus.Active)
            .ToListAsync();
    }
}
