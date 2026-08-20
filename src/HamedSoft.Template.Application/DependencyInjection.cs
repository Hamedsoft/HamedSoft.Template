using FluentValidation;
using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Application.Settings;
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

        services.AddScoped<ISettingService, SettingService>();

        services.AddSingleton<ISettingDefinitionProvider, SettingDefinitionProvider>();

        return services;
    }
}