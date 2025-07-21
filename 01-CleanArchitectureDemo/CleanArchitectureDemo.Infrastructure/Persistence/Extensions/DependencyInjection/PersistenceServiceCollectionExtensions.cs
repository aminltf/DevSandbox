using CleanArchitectureDemo.Application.Common.Interfaces.Data;
using CleanArchitectureDemo.Application.Common.Interfaces.Repositories;
using CleanArchitectureDemo.Infrastructure.Persistence.Contexts;
using CleanArchitectureDemo.Infrastructure.Persistence.Repositories;
using CleanArchitectureDemo.Infrastructure.Persistence.Services;
using DevSandbox.Shared.Kernel.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Extensions.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<ApplicationContext>());

        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register Services
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IPDFService, PDFService>();

        return services;
    }
}
