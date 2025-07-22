namespace OtpDemo.BLL.Interfaces.Services;

public interface ISmsSenderService
{
    Task SendSmsAsync(string phoneNumber, string message);
}
