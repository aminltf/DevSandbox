using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagementDemo.Application.Common.Interfaces.Repositories;
using UserManagementDemo.Application.Common.Interfaces.Security;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Domain.Entities;
using UserManagementDemo.Infrastructure.Identity.Contexts;
using UserManagementDemo.Infrastructure.Identity.Repositories;
using UserManagementDemo.Infrastructure.Identity.Security;
using UserManagementDemo.Infrastructure.Identity.Services;

namespace UserManagementDemo.Infrastructure.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        // Register Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILoginLogRepository, LoginLogRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICredentialResetTokenRepository, CredentialResetTokenRepository>();
        services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();

        // Register Security
        services.AddScoped<ICustomPasswordHasher, CustomPasswordHasher>();

        // Register Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, FakeEmailService>();
        services.AddScoped<ISmsService, FakeSmsService>();

        return services;
    }
}
