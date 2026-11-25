using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using ActionIn.Authentication.Application.Dtos;
using ActionIn.IntegrationTests.Infrastructure;

namespace ActionIn.IntegrationTests.Authentication.Register;

[Collection(PostgreSqlCollection.Name)]
public class RegisterAccountTest(PostgreSqlFixture postgres) : IAsyncLifetime
{
    private const string RegisterAccountUrl = "/Authentication/Register";
    private const string ProblemJson = "application/problem+json";
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

        Assert.Equal(request.Email, email.To);

        await using var context = postgres.CreateDbContext();

        var exists = await context.Accounts.AnyAsync(a => a.Email.Address == request.Email);
        Assert.True(exists);
    }

    [Fact]
    public async Task Register_DuplicatedEmail_ReturnsFailureWithConflictStatusCode()
    {
        var request = CreateRequest();
        var client = _factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync(RegisterAccountUrl, request);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var secondResponse = await client.PostAsJsonAsync(RegisterAccountUrl, request with { Username = "otheruser" });

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
        Assert.Equal(ProblemJson, secondResponse.Content.Headers.ContentType?.MediaType);

        var problem = await secondResponse.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Email.AlreadyInUse", problem.Extensions["code"]?.ToString());

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(1, accounts);

        Assert.Single(_factory.EmailSender.SentMessages);
    }

    [Theory]
    [InlineData("v", "vi@gmail.com", "example123", "Username")]
    [InlineData("vitor", "invalid", "example123", "Email")]
    [InlineData("vitor", "vi@gmail.com", "abc", "Password")]
    public async Task Register_InvalidPayload_ReturnsFailureWithBadRequestStatusCode(
          string username, string email, string password, string invalidField)
    {
        var request = new RegisterAccountDto(username, email, password);
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(RegisterAccountUrl, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Account.InvalidData", problem.Extensions["code"]?.ToString());
        Assert.Contains(invalidField, problem.Errors.Keys);

        await using var context = postgres.CreateDbContext();
        var accounts = await context.Accounts.CountAsync(a => a.Email.Address == request.Email);
        Assert.Equal(0, accounts);

        Assert.Empty(_factory.EmailSender.SentMessages);
    }
}


