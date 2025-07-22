namespace OtpDemo.BLL.Entities;

public class OtpCode
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}
