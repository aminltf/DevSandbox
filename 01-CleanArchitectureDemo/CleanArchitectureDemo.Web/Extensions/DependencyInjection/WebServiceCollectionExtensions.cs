using CleanArchitectureDemo.Application.Extensions.DependencyInjection;
using CleanArchitectureDemo.Infrastructure.Persistence.Extensions.DependencyInjection;
using DevSandbox.Shared.Web.Extensions;

namespace CleanArchitectureDemo.Web.Extensions.DependencyInjection;

public static class WebServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Localization
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        // Register Dependencies Layers
        services.AddApplicationDependencies()
                .AddPersistenceDependencies(configuration);

        // Register API Versioning
        services.AddApiVersioningDependencies();

        return services;
    }
}
