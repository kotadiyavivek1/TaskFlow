using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Services.Implementation;
using TaskFlow.Services.Interfaces;

namespace TaskFlow.Services.DependencyInjection;

public class DependencyInjection
{
    public static IServiceCollection AddServices(IServiceCollection services)
    {
        // Register Services Here   
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        // Register Services Here   
        Infrastructure.DependencyInjection.DependencyInjection.AddInfrastructure(services);
        return services;
    }
}
