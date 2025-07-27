// See https://aka.ms/new-console-template for more information
using DesignPatterns.Strategy.Sample.Strategies;
using DesignPatterns.Strategy.Sample;

Console.WriteLine("== Strategy Pattern Sample ==");
decimal cartAmount = 500_000; // Example cart total: 500,000 Toman

// 1. No Discount
var cart = new Cart(cartAmount, new NoDiscountStrategy());
Console.WriteLine($"No Discount: {cart.CalculateTotal():N0}");

// 2. 10% Percentage Discount
cart.SetDiscountStrategy(new PercentageDiscountStrategy(10));
Console.WriteLine($"10% Discount: {cart.CalculateTotal():N0}");

// 3. 50,000 Toman Fixed Discount
cart.SetDiscountStrategy(new FixedDiscountStrategy(50_000));
Console.WriteLine($"50,000 Toman Fixed Discount: {cart.CalculateTotal():N0}");
