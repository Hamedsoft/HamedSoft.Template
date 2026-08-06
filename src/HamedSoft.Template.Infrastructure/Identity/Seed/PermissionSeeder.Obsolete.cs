using HamedSoft.Template.Application.Security;
using HamedSoft.Template.Infrastructure.Identity.Models;
using HamedSoft.Template.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HamedSoft.Template.Infrastructure.Identity.Seed;

internal static class PermissionSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context)
    {
        //var permissions = new[]
        //{
        //    PermissionConstants.Roles.View,
        //    PermissionConstants.Roles.Create,
        //    PermissionConstants.Roles.Edit,
        //    PermissionConstants.Roles.Delete,

        //    PermissionConstants.Users.View,
        //    PermissionConstants.Users.Create,
        //    PermissionConstants.Users.Edit,
        //    PermissionConstants.Users.Delete
        //};


        //foreach (var permissionName in permissions)
        //{
        //    var exists = await context.Permissions
        //        .AnyAsync(
        //            x => x.Name == permissionName);


        //    if (!exists)
        //    {
        //        context.Permissions.Add(
        //            new Permission(permissionName));
        //    }
        //}


        //await context.SaveChangesAsync();
    }
}