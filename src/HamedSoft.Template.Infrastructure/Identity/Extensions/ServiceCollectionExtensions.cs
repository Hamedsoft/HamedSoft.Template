using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.Services;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Services;
using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.Infrastructure.Repositories.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HamedSoft.Template.Infrastructure.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IAuthenticationService, IdentityService>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<IAuthenticationService, IdentityService>();
        services.AddScoped<IRoleManagementService, RoleManagementService>();

        services.AddScoped<IRoleReadRepository, RoleReadRepository>();
        services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();

        return services;
    }
}