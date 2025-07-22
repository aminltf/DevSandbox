using DesignPatterns.UnitOfWork.Sample.Entities;

namespace DesignPatterns.UnitOfWork.Sample.Data;

public class InMemoryDbContext
{
    public List<Customer> Customers { get; set; } = new List<Customer>();

    // For simulation of commit
    public void SaveChanges()
    {
        // In real scenarios, here EF Core's SaveChanges would be called.
        // For in-memory, nothing needed.
    }
}
