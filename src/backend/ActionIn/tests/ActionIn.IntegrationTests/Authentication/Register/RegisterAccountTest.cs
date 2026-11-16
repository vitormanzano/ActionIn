using System.Net.Http.Json;
using ActionIn.Authentication.Application.Dtos;
using ActionIn.IntegrationTests.Infrastructure;

namespace ActionIn.IntegrationTests.Authentication.Register;

[Collection(PostgreSqlCollection.Name)]
public class RegisterAccountTest(PostgreSqlFixture postgres) : IAsyncLifetime
{
    private const string RegisterAccountUrl = "/Authentication/Register";
    private readonly IntegrationTestWebAppFactory _factory = new(postgres.ConnectionString);

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _factory.DisposeAsync();

    private static RegisterAccountDto CreateRequest()
    {
        return new RegisterAccountDto
            (Username: "vitor", Email: "vitor@example.com", Password: "Secret123");
    }

    [Fact]
    public async Task Success()
    {
        var request = CreateRequest();
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(RegisterAccountUrl, request);

        response.EnsureSuccessStatusCode();

    }
}


