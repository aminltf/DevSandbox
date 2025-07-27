namespace DesignPatterns.Strategy.Sample.Strategies;

public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal amount);
}
