namespace UserManagementDemo.Application.Common.Interfaces.Services;

public interface ISmsService
{
    Task SendAsync(string to, string message);
}
