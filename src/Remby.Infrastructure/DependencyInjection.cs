using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Remby.Infrastructure.Data;

namespace Remby.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NpgsqlConnection>(_ => 
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))
        );
        
        DatabaseInitializer.Initializer(configuration);
        
        return services;
    }
}