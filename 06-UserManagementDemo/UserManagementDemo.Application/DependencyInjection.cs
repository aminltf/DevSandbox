using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserManagementDemo.Application.Features.Users.Profiles;

namespace UserManagementDemo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        // Register Commands and Queries
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register Mappings
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<UserProfile>();
        });

        // Register Validations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
