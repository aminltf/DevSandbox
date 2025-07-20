#nullable disable

using CleanArchitectureDemo.Infrastructure.Persistence.Contexts;
using DevSandbox.Shared.Kernel.Abstractions.Repositories;
using DevSandbox.Shared.Kernel.Base.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseSoftDeletableEntity
{
    protected readonly ApplicationContext _context;

    public GenericRepository(ApplicationContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> AsQueryable(bool includeDeleted = false)
    {
        var query = _context.Set<TEntity>().AsQueryable();

        if (includeDeleted)
            query = query.IgnoreQueryFilters();

        else
            query = query.Where(x => !x.IsDeleted);

        return query;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllDeletedAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>()
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllDeletedAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        return entity;
    }

    public async Task<TEntity> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

        return entity;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().AnyAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);

        if (entity is null) return;

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.LastModifiedAt = DateTime.UtcNow;

        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<TEntity>()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (entity is null) return;

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.LastModifiedAt = DateTime.UtcNow;

        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
