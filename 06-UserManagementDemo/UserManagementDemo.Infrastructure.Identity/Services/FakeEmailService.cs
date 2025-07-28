using UserManagementDemo.Application.Common.Interfaces.Services;

namespace UserManagementDemo.Infrastructure.Identity.Services;

public class FakeEmailService : IEmailService
{
    public Task SendAsync(string to, string subject, string body)
    {
        Console.WriteLine($"[EMAIL MOCK] To: {to}\nSubject: {subject}\nBody: {body}");
        // or:
        // Debug.WriteLine($"[EMAIL MOCK] To: {to} | Subject: {subject} | Body: {body}");
        return Task.CompletedTask;
    }
}
