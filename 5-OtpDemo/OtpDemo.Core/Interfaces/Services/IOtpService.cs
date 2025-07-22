namespace OtpDemo.Core.Interfaces.Services;

public interface IOtpService
{
    Task GenerateAndSendOtpAsync(string phoneNumber);
    Task<bool> ValidateOtpAsync(string phoneNumber, string code);
}
