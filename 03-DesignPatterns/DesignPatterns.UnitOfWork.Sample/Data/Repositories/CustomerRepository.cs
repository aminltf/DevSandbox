using DesignPatterns.UnitOfWork.Sample.Data;
using DesignPatterns.UnitOfWork.Sample.Entities;

namespace DesignPatterns.UnitOfWork.Sample.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly InMemoryDbContext _context;

    public CustomerRepository(InMemoryDbContext context) => _context = context;

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public Customer? GetById(Guid id)
    {
        return _context.Customers.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<Customer> GetAll()
    {
        return _context.Customers;
    }

    public void Remove(Guid id)
    {
        var customer = GetById(id);
        if (customer != null)
            _context.Customers.Remove(customer);
    }
}
