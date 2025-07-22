using DesignPatterns.CQRS.Sample.Entities;

namespace DesignPatterns.CQRS.Sample.Commands;

public class CreateProductCommand
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}

public class CreateProductCommandHandler
{
    private readonly List<Product> _db;

    public CreateProductCommandHandler(List<Product> db)
    {
        _db = db;
    }

    public Product Handle(CreateProductCommand command)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Price = command.Price
        };
        _db.Add(product);
        return product;
    }
}
