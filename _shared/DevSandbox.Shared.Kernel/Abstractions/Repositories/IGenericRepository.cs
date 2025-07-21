using System.Linq.Expressions;
using DevSandbox.Shared.Kernel.Base.Entities;

namespace DevSandbox.Shared.Kernel.Abstractions.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseSoftDeletableEntity
{
    IQueryable<TEntity> AsQueryable(bool includeDeleted = false);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllDeletedAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllDeletedAsNoTrackingAsync(CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TEntity> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task RestoreAsync(Guid id, CancellationToken cancellationToken);
}
