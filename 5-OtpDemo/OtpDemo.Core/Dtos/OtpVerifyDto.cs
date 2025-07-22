namespace OtpDemo.Core.Dtos;

public class OtpVerifyDto
{
    public string PhoneNumber { get; set; } = null!;
    public string Code { get; set; } = null!;
}
