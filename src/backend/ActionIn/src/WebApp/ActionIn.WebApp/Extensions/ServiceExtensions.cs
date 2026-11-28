using System.Reflection;
using ActionIn.Core.Infrastructure.Behaviors;
using ActionIn.Core.Infrastructure.Interceptor;
using ActionIn.Core.Infrastructure.Mediatr;

namespace ActionIn.WebApp.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDbInterceptor(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventInterceptor>();

        return services;
    }

    public static IServiceCollection Mediator(this IServiceCollection services, string? licenseKey, params Assembly[] moduleAssemblies)
    {
        services.AddScoped<IMediatrHandler, MediatrHandler>();

        services.AddMediatR(cfg =>
            {
                cfg.LicenseKey = licenseKey;
                cfg.RegisterServicesFromAssemblies(moduleAssemblies);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

        return services;
    }
}


