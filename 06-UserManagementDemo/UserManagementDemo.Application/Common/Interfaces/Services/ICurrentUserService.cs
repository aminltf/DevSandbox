using UserManagementDemo.Application.Common.Models;

namespace UserManagementDemo.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    SessionInfo Session { get; }
}
