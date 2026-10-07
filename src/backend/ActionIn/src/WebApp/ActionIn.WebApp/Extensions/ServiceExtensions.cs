using ActionIn.Authentication.Application.Commands.RegisterAccount;
using ActionIn.Authentication.Data;
using ActionIn.Authentication.Data.Repository;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Core.Mediatr;
using ActionIn.Core.Messages.Commands;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.WebApp.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IServiceCollection Mediator(this IServiceCollection services)
    {
        services.AddScoped<IMediatrHandler, MediatrHandler>();

        services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(MediatrHandler).Assembly);
            });

        return services;
    }

    public static IServiceCollection AddCustomCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<RegisterAccountCommand>, RegisterAccountCommandHandler>();

        return services;
    }
}


