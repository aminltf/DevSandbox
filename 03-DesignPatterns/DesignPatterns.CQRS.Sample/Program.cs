// See https://aka.ms/new-console-template for more information
using DesignPatterns.CQRS.Sample.Commands;
using DesignPatterns.CQRS.Sample.Data;
using DesignPatterns.CQRS.Sample.Queries;

var dbContext = new InMemoryDbContext();

var createHandler = new CreateProductCommandHandler(dbContext.Products);
var queryHandler = new GetProductByIdQueryHandler(dbContext.Products);

var product = createHandler.Handle(new CreateProductCommand { Name = "Laptop", Price = 30_000_000 });
Console.WriteLine($"Created Product: {product.Id} - {product.Name}");

var fetched = queryHandler.Handle(new GetProductByIdQuery { ProductId = product.Id });
Console.WriteLine($"Fetched Product: {fetched?.Name} - {fetched?.Price}");
