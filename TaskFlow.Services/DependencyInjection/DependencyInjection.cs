using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Services.Services.Implementation;
using TaskFlow.Services.Services.Interfaces;
using TaskFlow.Shared.Settings;

namespace TaskFlow.Services.DependencyInjection;

public class DependencyInjection
{
    public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        // Bind JWT settings from appsettings.json
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Register services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService,  AuthService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(IServiceCollection services)
    {
        Infrastructure.DependencyInjection.DependencyInjection.AddInfrastructure(services);
        return services;
    }
}
