#nullable disable

using System.Collections;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Infrastructure.Persistence.Contexts;
using DevSandbox.Shared.Kernel.Abstractions.Repositories;
using DevSandbox.Shared.Kernel.Base.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;
    private Hashtable _repositories;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(ApplicationContext context, IProductRepository product)
    {
        _context = context;
        Product = product;
    }

    public IProductRepository Product { get; }

    public IGenericRepository<T> Repository<T>() where T : BaseSoftDeletableEntity
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<T>);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction == null)
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null)
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        _currentTransaction?.Dispose();
    }
}
