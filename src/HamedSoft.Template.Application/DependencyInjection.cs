using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HamedSoft.Template.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);

            cfg.AddOpenBehavior(typeof(Behaviors.ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

        return services;
    }
}