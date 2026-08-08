using HamedSoft.Template.Application.Security;

namespace HamedSoft.Template.Infrastructure.Identity.Permissions;

public static class PermissionCatalog
{
    public static IReadOnlyCollection<PermissionDefinition> All { get; } =
    [
        new(
            PermissionConstants.Users.View,
            "Administration",
            "Users",
            "مشاهده لیست کاربران",
            "امکان مشاهده فهرست کاربران"),

        new(
            PermissionConstants.Users.Create,
            "Administration",
            "Users",
            "ایجاد کاربر",
            "امکان ایجاد کاربر جدید"),

        new(
            PermissionConstants.Users.Edit,
            "Administration",
            "Users",
            "ویرایش کاربر",
            "امکان ویرایش اطلاعات کاربر"),

        new(
            PermissionConstants.Users.Delete,
            "Administration",
            "Users",
            "حذف کاربر",
            "امکان حذف کاربر"),

        new(
            PermissionConstants.Roles.View,
            "Administration",
            "Roles",
            "مشاهده لیست نقش‌ها",
            "امکان مشاهده فهرست نقش‌ها"),

        new(
            PermissionConstants.Roles.Create,
            "Administration",
            "Roles",
            "ایجاد نقش",
            "امکان ایجاد نقش جدید"),

        new(
            PermissionConstants.Roles.Edit,
            "Administration",
            "Roles",
            "ویرایش نقش",
            "امکان ویرایش نقش"),

        new(
            PermissionConstants.Roles.Delete,
            "Administration",
            "Roles",
            "حذف نقش",
            "امکان حذف نقش"),

        new(
            PermissionConstants.Settings.View,
            "Administration",
            "Settings",
            "مشاهده تنظیمات",
            "امکان مشاهده تنظیمات سیستم"),

        new(
            PermissionConstants.Settings.Edit,
            "Administration",
            "Settings",
            "ویرایش تنظیمات",
            "امکان ویرایش تنظیمات سیستم")
    ];
}