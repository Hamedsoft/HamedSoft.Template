using Microsoft.Extensions.DependencyInjection;

namespace HamedSoft.Template.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        return services;
    }
}
