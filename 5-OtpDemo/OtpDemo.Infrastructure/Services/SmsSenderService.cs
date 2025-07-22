using OtpDemo.Core.Interfaces.Services;

namespace OtpDemo.Infrastructure.Services;

public class SmsSenderService : ISmsSenderService
{
    public Task SendSmsAsync(string phoneNumber, string message)
    {
        Console.WriteLine($"SMS to {phoneNumber}: {message}");
        return Task.CompletedTask;
    }
}
