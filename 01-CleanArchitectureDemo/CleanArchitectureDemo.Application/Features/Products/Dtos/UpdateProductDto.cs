namespace CleanArchitectureDemo.Application.Features.Products.Dtos;

public class UpdateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = null!;
    public int Status { get; set; }
    public int Stock { get; set; }
}
