using DesignPatterns.CQRS.Sample.Entities;

namespace DesignPatterns.CQRS.Sample.Queries;

public class GetProductByIdQuery
{
    public Guid ProductId { get; set; }
}

public class GetProductByIdQueryHandler
{
    private readonly List<Product> _db;

    public GetProductByIdQueryHandler(List<Product> db)
    {
        _db = db;
    }

    public Product? Handle(GetProductByIdQuery query)
    {
        return _db.FirstOrDefault(p => p.Id == query.ProductId);
    }
}
