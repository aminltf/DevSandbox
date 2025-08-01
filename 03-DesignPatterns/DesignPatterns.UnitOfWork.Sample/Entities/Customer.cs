﻿namespace DesignPatterns.UnitOfWork.Sample.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime RegisteredAt { get; set; }
}
