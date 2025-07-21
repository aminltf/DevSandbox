using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Domain.Enums;
using CleanArchitectureDemo.Infrastructure.Persistence.Contexts;
using DevSandbox.Shared.Kernel.Extensions;
using DevSandbox.Shared.Kernel.Paging;
using DevSandbox.Shared.Kernel.Search;
using DevSandbox.Shared.Kernel.Sorting;
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

    public async Task<PageResponse<Product>> GetProductsAsync(
        PageRequest page,
        SearchRequest search,
        SortOptions sort,
        CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsQueryable()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        // Search
        if (!string.IsNullOrWhiteSpace(search?.SearchTerm))
        {
            var term = $"%{search.SearchTerm.Trim()}%";
            query = query.Where(x =>
                EF.Functions.Like(x.CreatedAt.ToString(), term) ||
                EF.Functions.Like(x.Name, term) ||
                EF.Functions.Like(x.Price.Amount.ToString(), term) ||
                EF.Functions.Like(x.Status.ToString(), term) ||
                EF.Functions.Like(x.Stock.ToString(), term)
            );
        }

        // Sorting
        query = query.ApplySorting(sort);

        // Count
        var totalCount = await query.CountAsync(cancellationToken);

        // Paging + Projection
        var items = await query
            .ApplyPaging(page)
            .ToListAsync(cancellationToken);

        return new PageResponse<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = page.PageNumber,
            PageSize = page.PageSize,
        };
    }
}
