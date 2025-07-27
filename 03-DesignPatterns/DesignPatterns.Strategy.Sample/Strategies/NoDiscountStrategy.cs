namespace DesignPatterns.Strategy.Sample.Strategies;

public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal amount) => amount;
}
