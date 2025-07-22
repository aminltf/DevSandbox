using DesignPatterns.CQRS.Sample.Entities;

namespace DesignPatterns.CQRS.Sample.Data;

public class InMemoryDbContext
{
    public List<Product> Products { get; set; } = new List<Product>();
}
