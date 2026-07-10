using HamedSoft.Template.Application.Abstractions.Authentication;
using HamedSoft.Template.Application.Abstractions.Common;
using HamedSoft.Template.Infrastructure.Common;
using HamedSoft.Template.Infrastructure.Identity;
using HamedSoft.Template.Infrastructure.Persistence;
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

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });


        services.AddIdentityServices();


        return services;
    }
}