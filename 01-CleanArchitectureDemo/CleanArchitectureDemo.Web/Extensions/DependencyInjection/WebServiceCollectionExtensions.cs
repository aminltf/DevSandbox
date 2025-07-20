using DevSandbox.Shared.Web.Extensions;

namespace CleanArchitectureDemo.Web.Extensions.DependencyInjection;

public static class WebServiceCollectionExtensions
{
    #region Extension Method
    public static IServiceCollection AddServices(this IServiceCollection services, IHostBuilder host, IConfiguration configuration)
    {
        // Register Localization
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        // Register Dependencies Layers

        // Register API Versioning
        services.AddApiVersioningDependencies();

        return services;
    }
    #endregion
}
