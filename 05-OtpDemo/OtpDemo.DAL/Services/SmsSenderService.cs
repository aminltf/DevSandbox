using OtpDemo.BLL.Interfaces.Services;

namespace OtpDemo.DAL.Services;

public class SmsSenderService : ISmsSenderService
{
    public Task SendSmsAsync(string phoneNumber, string message)
    {
        Console.WriteLine($"SMS to {phoneNumber}: {message}");
        return Task.CompletedTask;
    }
}
