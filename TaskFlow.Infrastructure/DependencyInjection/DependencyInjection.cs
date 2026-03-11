using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repositories.Implementations;
using TaskFlow.Infrastructure.Repositories.Interfaces;

namespace TaskFlow.Infrastructure.DependencyInjection;

public class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        // One Generic Repository registration per entity — no custom repos needed
        services.AddScoped<IGenericRepository<User>,         GenericRepository<User>>();
        services.AddScoped<IGenericRepository<Role>,         GenericRepository<Role>>();
        services.AddScoped<IGenericRepository<UserRole>,     GenericRepository<UserRole>>();
        services.AddScoped<IGenericRepository<RefreshToken>, GenericRepository<RefreshToken>>();
        services.AddScoped<IGenericRepository<Project>,      GenericRepository<Project>>();
        services.AddScoped<IGenericRepository<TaskItem>,     GenericRepository<TaskItem>>();
        services.AddScoped<IGenericRepository<TaskComment>,  GenericRepository<TaskComment>>();

        return services;
    }
}
