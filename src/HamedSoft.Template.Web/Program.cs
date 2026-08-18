using HamedSoft.Template.Application;
using HamedSoft.Template.Application.Contracts.Security;
using HamedSoft.Template.Infrastructure;
using HamedSoft.Template.Infrastructure.Initialization;
using HamedSoft.Template.Web.ErrorHandling;
using HamedSoft.Template.Web.Middleware;
using HamedSoft.Template.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration( context.Configuration));

builder.Services.AddControllersWithViews();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddExceptionHandling();

builder.Services.AddAuthentication(
    IdentityConstants.ApplicationScheme);

builder.Services.AddAuthorization();

builder.Services.AddSingleton<
    IAuthorizationPolicyProvider,
    PermissionPolicyProvider>();

builder.Services.AddScoped<
    IAuthorizationHandler,
    PermissionAuthorizationHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var initializer =
        scope.ServiceProvider
            .GetRequiredService<InfrastructureInitializer>();

    await initializer.InitializeAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCorrelationId();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseMiddleware<RequestLoggingContextMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();