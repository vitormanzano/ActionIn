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

        Assert.Equal(request.Email, email.To.Address);

        await using var context = postgres.CreateDbContext();

        var exists = await context.Accounts.AnyAsync(a => a.Email.Address == request.Email);
        Assert.True(exists);
    }

    [Fact]
    public async Task Register_DuplicatedEmail_ThrowsException()
    {
        // Characterization: today the API throws on a duplicated e-mail.
        var request = CreateRequest();
        var client = _factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync(RegisterAccountUrl, request);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var exception = await Assert.ThrowsAsync<Exception>(
                () => client.PostAsJsonAsync(RegisterAccountUrl, request));

        Assert.Equal("Email already exists", exception.Message);

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(1, accounts);

        Assert.Single(_factory.EmailSender.SentMessages);
    }

    [Theory]
    [InlineData("v", "vi@gmail.com", "example123", "Username must have at least 4 characters.")]
    [InlineData("vitor", "invalid", "example123", "Email format is invalid.")]
    [InlineData("vitor", "vi@gmail.com", "abc", "Password must have at least 4 characters.")]
    public async Task Register_InvalidPayload_ThrowsException(
          string username, string email, string password, string expectedMessage)
    {
        var request = new RegisterAccountDto(username, email, password);
        var client = _factory.CreateClient();

        var exception = await Assert.ThrowsAsync<Exception>(
                () => client.PostAsJsonAsync(RegisterAccountUrl, request));

        Assert.Equal(expectedMessage, exception.Message);

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(0, accounts);

        Assert.Empty(_factory.EmailSender.SentMessages);
    }
}


