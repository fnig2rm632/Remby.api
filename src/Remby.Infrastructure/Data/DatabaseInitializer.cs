using DbUp;
using Microsoft.Extensions.Configuration;

namespace Remby.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static void Initializer(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);
        
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseInitializer).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();
        
        var result = upgrader.PerformUpgrade();
        
        if(!result.Successful)
            throw new Exception($"Database Upgrade Failed: {result.Error}");
    }
}