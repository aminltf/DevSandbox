namespace UserManagementDemo.Application.Common.Models;

public class JwtSettings
{
    public string Secret { get; set; } = default!;
    public int TokenLifetimeSeconds { get; set; }
    public int RefreshTokenLifetimeDays { get; set; }
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
}
