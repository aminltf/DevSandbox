namespace CleanArchitectureDemo.Application.Features.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = null!;
    public int Status { get; set; }
    public string StatusTitle { get; set; } = null!;
    public int Stock { get; set; }
}
