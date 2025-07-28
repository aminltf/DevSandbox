using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Common.Models;

namespace UserManagementDemo.Infrastructure.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public SessionInfo Session
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var info = new SessionInfo();

            info.IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;

            if (info.IsAuthenticated)
            {
                var userIdClaim = user!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdClaim, out var userId))
                    info.UserId = userId;

                info.UserName = user.FindFirst(ClaimTypes.Name)?.Value;
                info.Email = user.FindFirst(ClaimTypes.Email)?.Value;
                info.FirstName = user.FindFirst("first_name")?.Value;
                info.LastName = user.FindFirst("last_name")?.Value;
                info.Role = user.FindFirst(ClaimTypes.Role)?.Value;

                info.IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }

            return info;
        }
    }
}
