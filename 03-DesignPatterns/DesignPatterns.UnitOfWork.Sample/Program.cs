using DesignPatterns.UnitOfWork.Sample.Data;
using DesignPatterns.UnitOfWork.Sample.Data.UnitOfWork;
using DesignPatterns.UnitOfWork.Sample.Entities;

// Initialize in-memory database and unit of work
var dbContext = new InMemoryDbContext();
var unitOfWork = new UnitOfWork(dbContext);

// Add a new customer
var customer = new Customer
{
    Id = Guid.NewGuid(),
    FullName = "Amin Lotfi",
    Email = "amin.lotfi@example.com",
    RegisteredAt = DateTime.UtcNow
};
unitOfWork.Customers.Add(customer); // Add customer to repository

// Commit the changes (simulate SaveChanges)
unitOfWork.Commit();

Console.WriteLine("Customer added and committed!");

// List all customers after adding
foreach (var item in unitOfWork.Customers.GetAll())
{
    Console.WriteLine($"{item.Id} - {item.FullName} - {item.Email} - Registered at: {item.RegisteredAt}");
}

// Remove a customer by ID
unitOfWork.Customers.Remove(customer.Id); // Remove customer from repository
unitOfWork.Commit(); // Commit changes

Console.WriteLine("Customer removed and committed!");

// List customers again (should be empty now)
foreach (var item in unitOfWork.Customers.GetAll())
{
    Console.WriteLine($"{item.Id} - {item.FullName} - {item.Email}");
}
