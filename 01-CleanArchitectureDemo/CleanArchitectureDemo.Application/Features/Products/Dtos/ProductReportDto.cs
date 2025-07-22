namespace CleanArchitectureDemo.Application.Features.Products.Dtos;

public class ProductReportDto
{
    public string Name { get; set; } = null!;
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = null!;
    public string StatusTitle { get; set; } = null!;
    public int Stock { get; set; }
}
