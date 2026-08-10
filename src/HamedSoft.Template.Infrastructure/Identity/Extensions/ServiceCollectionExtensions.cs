using HamedSoft.Template.Application.Contracts.Authentication;
using HamedSoft.Template.Application.Contracts.Permissions;
using HamedSoft.Template.Application.Contracts.Repositories.Reads;
using HamedSoft.Template.Application.Contracts.Repositories.Writes;
using HamedSoft.Template.Application.Contracts.Roles;
using HamedSoft.Template.Application.Contracts.Users;
using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Identity.Options;
using HamedSoft.Template.Infrastructure.Identity.Permissions;
using HamedSoft.Template.Infrastructure.Identity.Services;
using HamedSoft.Template.Infrastructure.Persistence;
using HamedSoft.Template.Infrastructure.Repositories.Permissions;
using HamedSoft.Template.Infrastructure.Repositories.Roles;
using HamedSoft.Template.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HamedSoft.Template.Infrastructure.Identity.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
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

        services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.LoginPath = "/Account/Login";
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IAuthenticationService, IdentityService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleManagementService, RoleManagementService>();

        services.AddScoped<IRoleReadRepository, RoleReadRepository>();
        services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();

        services.Configure<AdminUserOptions>(configuration.GetSection(AdminUserOptions.SectionName));

        services.AddScoped<IUserManagementService, UserManagementService>();

        services.AddScoped<IPermissionDiscoveryService, PermissionDiscoveryService>();

        services.AddScoped<PermissionSynchronizer>();

        services.AddScoped<IPermissionChecker, PermissionChecker>();

        services.AddScoped<IPermissionReadRepository, PermissionReadRepository>();

        return services;
    }
}