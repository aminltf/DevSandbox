using CleanArchitectureDemo.Domain.Enums;
using CleanArchitectureDemo.Domain.ValueObjects;
using DevSandbox.Shared.Kernel.Base.Entities;

namespace CleanArchitectureDemo.Domain.Entities;

public class Product : BaseSoftDeletableEntity
{
    public string Name { get; set; } = null!;
    public Money Price { get; set; } = null!;
    public ProductStatus Status { get; set; }
    public int Stock { get; set; }

    public Product() { }

    public Product(string name, Money price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
        Status = ProductStatus.Active;
    }

    public void UpdateStock(int quantity) => Stock = Math.Max(0, Stock + quantity);

    public void ChangeStatus(ProductStatus status) => Status = status;
}
