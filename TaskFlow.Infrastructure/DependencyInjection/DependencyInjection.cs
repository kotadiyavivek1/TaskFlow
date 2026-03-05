using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories.Implementations;
using TaskFlow.Infrastructure.Repositories.Interfaces;
namespace TaskFlow.Infrastructure.DependencyInjection;

public class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        // Register Services Here   
        services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
        return services;
    }
}
