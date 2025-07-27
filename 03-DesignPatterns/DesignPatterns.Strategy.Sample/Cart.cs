using DesignPatterns.Strategy.Sample.Strategies;

namespace DesignPatterns.Strategy.Sample;

public class Cart
{
    public decimal Amount { get; set; }
    private IDiscountStrategy _discountStrategy;

    public Cart(decimal amount, IDiscountStrategy discountStrategy)
    {
        Amount = amount;
        _discountStrategy = discountStrategy;
    }

    public void SetDiscountStrategy(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal CalculateTotal()
    {
        return _discountStrategy.ApplyDiscount(Amount);
    }
}
