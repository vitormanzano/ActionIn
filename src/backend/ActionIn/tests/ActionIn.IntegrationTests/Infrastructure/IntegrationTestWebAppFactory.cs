using ActionIn.Authentication.AntiCorruption;
using ActionIn.Authentication.Domain.Notifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ActionIn.IntegrationTests.Infrastructure;

public sealed class IntegrationTestWebAppFactory(string connectionString) : WebApplicationFactory<Program>
{
    public FakeEmailSender EmailSender { get; } = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.UseSetting("ConnectionStrings:DefaultConnection", connectionString);

        builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IEmailSender>();
                services.AddSingleton<IEmailSender>(EmailSender);
            });
    }
}


