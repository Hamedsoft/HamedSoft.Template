using HamedSoft.Template.Application.Abstractions.Common;
using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.UnitOfWork;
using HamedSoft.Template.Infrastructure.Common;
using HamedSoft.Template.Infrastructure.Identity.Extensions;
using HamedSoft.Template.Infrastructure.Identity.Services;
using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.Infrastructure.Persistence.Interceptors;
using HamedSoft.Template.Infrastructure.Persistence.Repositories.UserProfiles;
using HamedSoft.Template.Infrastructure.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HamedSoft.Template.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

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
        
        services.AddScoped<IAuthenticationService, IdentityService>();

        services.AddScoped<IRoleManagementService, RoleManagementService>();

        services.AddScoped<IUserProfileWriteRepository, UserProfileWriteRepository>();

        services.AddScoped<IUserProfileReadRepository, UserProfileReadRepository>();
        
        services.AddScoped<IApplicationUnitOfWork, ApplicationUnitOfWork>();

        services.AddIdentityServices(configuration);

        return services;
    }
}