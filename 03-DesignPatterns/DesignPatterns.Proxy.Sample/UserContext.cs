namespace DesignPatterns.Proxy.Sample;

public enum Role { Employee, Manager, Admin }

public sealed class UserContext
{
    public required Guid UserId { get; init; }
    public required Role Role { get; init; }
}