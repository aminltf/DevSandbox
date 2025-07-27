namespace DesignPatterns.Strategy.Sample.Strategies;

public class PercentageDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _percent;
    public PercentageDiscountStrategy(decimal percent)
    {
        _percent = percent;
    }
    public decimal ApplyDiscount(decimal amount)
    {
        return amount - (amount * _percent / 100);
    }
}
