using ActionIn.Authentication.Application.Services;
using ActionIn.Authentication.Data;
using ActionIn.Authentication.Data.Repository;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.WebApp.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}


