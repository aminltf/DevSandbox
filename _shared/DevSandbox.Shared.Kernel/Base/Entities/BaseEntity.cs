using DevSandbox.Shared.Kernel.Abstractions.Entities;

namespace DevSandbox.Shared.Kernel.Base.Entities;

public abstract class BaseEntity : IEntity<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
