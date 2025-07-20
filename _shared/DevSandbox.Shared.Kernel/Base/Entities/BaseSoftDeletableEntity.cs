using DevSandbox.Shared.Kernel.Abstractions.Entities;

namespace DevSandbox.Shared.Kernel.Base.Entities;

public abstract class BaseSoftDeletableEntity : BaseAuditableEntity, ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}
