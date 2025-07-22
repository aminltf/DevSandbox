using DesignPatterns.UnitOfWork.Sample.Data;
using DesignPatterns.UnitOfWork.Sample.Data.Repositories;

namespace DesignPatterns.UnitOfWork.Sample.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly InMemoryDbContext _context;
    public ICustomerRepository Customers { get; }

    public UnitOfWork(InMemoryDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
    }

    public void Commit()
    {
        _context.SaveChanges();
    }
}
