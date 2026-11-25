using ActionIn.Authentication.Data;
using ActionIn.Core.Infrastructure.Interceptor;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace ActionIn.IntegrationTests.Infrastructure;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly FakeMediatrHandler _fakeMediatrHandler = new();
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .Build();

    public string ConnectionString => _postgres.GetConnectionString();

    public AuthenticationContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AuthenticationContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new AuthenticationContext(options, new PublishDomainEventInterceptor(_fakeMediatrHandler));
    }
    public async Task CleanTableAccountsAsync()
    {

        await _postgres.ExecScriptAsync("DELETE FROM \"Accounts\"");
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();


        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();

    }
}


