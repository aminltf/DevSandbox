namespace DesignPatterns.Strategy.Sample.Strategies;

public class FixedDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _fixedAmount;
    public FixedDiscountStrategy(decimal fixedAmount)
    {
        _fixedAmount = fixedAmount;
    }
    public decimal ApplyDiscount(decimal amount)
    {
        return amount - _fixedAmount;
    }
}
