namespace OtpDemo.Core.Interfaces.Services;

public interface ISmsSenderService
{
    Task SendSmsAsync(string phoneNumber, string message);
}
