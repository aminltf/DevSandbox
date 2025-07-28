using UserManagementDemo.Application.Common.Interfaces.Services;

namespace UserManagementDemo.Infrastructure.Identity.Services;

public class FakeSmsService : ISmsService
{
    public Task SendAsync(string to, string message)
    {
        Console.WriteLine($"[SMS MOCK] To: {to}\nMessage: {message}");
        // or:
        // Debug.WriteLine($"[SMS MOCK] To: {to} | Message: {message}");
        return Task.CompletedTask;
    }
}
