using CompraProgramada.Application.SharedContext.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CompraProgramada.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);
        return services;
    }
}