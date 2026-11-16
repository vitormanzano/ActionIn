using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using ActionIn.Authentication.Application.Dtos;
using ActionIn.IntegrationTests.Infrastructure;

namespace ActionIn.IntegrationTests.Authentication.Register;

[Collection(PostgreSqlCollection.Name)]
public class RegisterAccountTest(PostgreSqlFixture postgres) : IAsyncLifetime
{
    private const string RegisterAccountUrl = "/Authentication/Register";
    private readonly IntegrationTestWebAppFactory _factory = new(postgres.ConnectionString);

    public Task InitializeAsync() => postgres.CleanTableAccountsAsync();

    public async Task DisposeAsync() => await _factory.DisposeAsync();

    private static RegisterAccountDto CreateRequest()
    {
        return new RegisterAccountDto
            (Username: "vitor", Email: "vitor@example.com", Password: "Secret123");
    }

    [Fact]
    public async Task Register_ShouldInsertANewAccount_AndSendAnEmail()
    {
        var request = CreateRequest();
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(RegisterAccountUrl, request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var email = Assert.Single(_factory.EmailSender.SentMessages);

        Assert.Equal(request.Email, email.To.Value);

        await using var context = postgres.CreateDbContext();

        var exists = await context.Accounts.AnyAsync(a => a.Email.Value == request.Email);
        Assert.True(exists);
    }
}


