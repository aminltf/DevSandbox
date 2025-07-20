namespace DevSandbox.Shared.Kernel.Abstractions.Repositories;

public interface IBaseUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
