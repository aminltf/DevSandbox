namespace DevSandbox.Shared.Kernel.Abstractions.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}
