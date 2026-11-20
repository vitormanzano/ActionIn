using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using ActionIn.Authentication.Application.Dtos;
using ActionIn.IntegrationTests.Infrastructure;
using ActionIn.Core.Results;
using ActionIn.Authentication.Domain;

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

        Assert.Equal(request.Email, email.To.Address);

        await using var context = postgres.CreateDbContext();

        var exists = await context.Accounts.AnyAsync(a => a.Email.Address == request.Email);
        Assert.True(exists);
    }

    [Fact]
    public async Task Register_DuplicatedEmail_ReturnsFailure()
    {
        // Characterization: today the API throws on a duplicated e-mail.
        var request = CreateRequest();
        var client = _factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync(RegisterAccountUrl, request);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondResponse = await client.PostAsJsonAsync(RegisterAccountUrl, request);

        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(1, accounts);

        Assert.Single(_factory.EmailSender.SentMessages);
    }

    [Theory]
    [InlineData("v", "vi@gmail.com", "example123")]
    [InlineData("vitor", "invalid", "example123")]
    [InlineData("vitor", "vi@gmail.com", "abc")]
    public async Task Register_InvalidPayload_ReturnsFailure(
          string username, string email, string password)
    {
        var request = new RegisterAccountDto(username, email, password);
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(RegisterAccountUrl, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(0, accounts);

        Assert.Empty(_factory.EmailSender.SentMessages);
    }
}


