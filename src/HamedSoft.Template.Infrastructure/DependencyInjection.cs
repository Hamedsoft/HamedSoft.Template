using HamedSoft.Template.Application.Abstractions.Common;
using HamedSoft.Template.Infrastructure.Identity.Extensions;
using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.Infrastructure.Persistence.Interceptors;
using HamedSoft.Template.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HamedSoft.Template.Application.Contracts.Common;

namespace HamedSoft.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddScoped<AuditableEntityInterceptor>();
        
        services.AddScoped<SoftDeleteInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var auditInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();
            var softDeleteInterceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();

            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(softDeleteInterceptor, auditInterceptor);
        });


        services.AddIdentityServices();


        return services;
    }
}