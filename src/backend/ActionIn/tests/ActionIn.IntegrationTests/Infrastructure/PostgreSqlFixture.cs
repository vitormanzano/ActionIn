using ActionIn.Authentication.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace ActionIn.IntegrationTests.Infrastructure;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .Build();

    public string ConnectionString => _postgres.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AuthenticationContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        await using var context = new AuthenticationContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}


