using System.Reflection;
using ActionIn.Authentication.AntiCorruption;
using ActionIn.Authentication.Application.Commands.RegisterAccount;
using ActionIn.Authentication.Application.Notifications;
using ActionIn.Authentication.Data;
using ActionIn.Authentication.Data.Repository;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Repository;
using ActionIn.Authentication.Domain.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ActionIn.WebApp.Extensions;

public static class AuthenticationModule
{
    public static readonly Assembly ApplicationAssembly = typeof(RegisterAccountCommandHandler).Assembly;

    public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, string connectionString, IConfiguration configuration)
    {
        services.AddCustomServices();
        services.AddDbCtx(connectionString);
        services.AddValidatorsFromAssembly(ApplicationAssembly);
        services.AddSmtp(configuration);

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        return services;
    }

    private static IServiceCollection AddDbCtx(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AuthenticationContext>(options => options.UseNpgsql(connectionString));

        return services;
    }


    private static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SmtpSettings>()
            .Bind(configuration.GetSection(SmtpSettings.SectionName))
            .ValidateDataAnnotations()
            .Validate(settings => string.IsNullOrEmpty(settings.Username) == string.IsNullOrEmpty(settings.Password),
                "Smtp:Username and Smtp:Password must be set together.")
            .ValidateOnStart();

        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}



