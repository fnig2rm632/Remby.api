using Microsoft.Extensions.Configuration;
using Npgsql;
using Remby.Infrastructure.Data;
using Testcontainers.PostgreSql;
using Xunit;

namespace Remby.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class TestInfrastructureFixture : IAsyncLifetime
{
    private PostgreSqlContainer Postgres { get; } = new PostgreSqlBuilder("postgres:16")
        .WithDatabase("remby_test")
        .WithUsername("remby_test_user")
        .WithPassword("remby_test_password")
        .Build();

    public async Task InitializeAsync()
    {
        await Postgres.StartAsync();
        InitializeSchema();
    }

    public async Task DisposeAsync()
    {
        await Postgres.DisposeAsync();
    }

    public async Task<NpgsqlConnection> OpenConnectionAsync()
    {
        var connection = new NpgsqlConnection(Postgres.GetConnectionString());
        await connection.OpenAsync();
        return connection;
    }

    private void InitializeSchema()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = Postgres.GetConnectionString()
            })
            .Build();

        DatabaseInitializer.Initializer(configuration);
    }
}
