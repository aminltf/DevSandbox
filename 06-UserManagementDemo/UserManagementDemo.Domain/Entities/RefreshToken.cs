using DevSandbox.Shared.Kernel.Base.Entities;

namespace UserManagementDemo.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = default!;
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public ApplicationUser User { get; set; } = default!;

    // helper property
    public bool IsActive => Revoked == null && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;

    public RefreshToken()
    {
        
    }
}
