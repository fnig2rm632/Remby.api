using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remby.Application.Interfaces.Command;
using Remby.Application.Interfaces.Query;
using Remby.Infrastructure.Data;
using Remby.Infrastructure.Persistence.Command;
using Remby.Infrastructure.Persistence.Query;
using Npgsql;

namespace Remby.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<NpgsqlConnection>(_ => 
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IFolderQueryRepository, FolderQueryRepository>();
        services.AddScoped<ICardQueryRepository, CardQueryRepositor>();
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IFolderCommandRepository, FolderCommandRepository>();
        services.AddScoped<ICardCommandRepository, CardCommandRepository>();

        DatabaseInitializer.Initializer(configuration);

        return services;
    }
}
