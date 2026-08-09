namespace HamedSoft.Template.Application.Security;

public static class PermissionConstants
{
    public static class Users
    {
        [PermissionDefinition("Administration", "Users", "مشاهده لیست کاربران", "امکان مشاهده فهرست کاربران")]
        public const string View = "Users.View";

        [PermissionDefinition("Administration", "Users", "ایجاد کاربر", "امکان ایجاد کاربر جدید")]
        public const string Create = "Users.Create";

        [PermissionDefinition("Administration", "Users", "ویرایش کاربر", "امکان ویرایش اطلاعات کاربر")]
        public const string Edit = "Users.Edit";

        [PermissionDefinition("Administration", "Users", "حذف کاربر", "امکان حذف کاربر")]
        public const string Delete = "Users.Delete";
    }

    public static class Roles
    {
        [PermissionDefinition("Administration", "Roles", "مشاهده لیست نقش‌ها", "امکان مشاهده فهرست نقش‌ها")]
        public const string View = "Roles.View";

        [PermissionDefinition("Administration", "Roles", "ایجاد نقش", "امکان ایجاد نقش جدید")]
        public const string Create = "Roles.Create";

        [PermissionDefinition("Administration", "Roles", "ویرایش نقش", "امکان ویرایش نقش")] 
        public const string Edit = "Roles.Edit";

        [PermissionDefinition("Administration", "Roles", "حذف نقش", "امکان حذف نقش")] 
        public const string Delete = "Roles.Delete";

        [PermissionDefinition("Administration", "Roles", "مدیریت دسترسی‌های نقش", "امکان تعیین دسترسی‌های یک نقش")]
        public const string AssignPermissions = "Roles.AssignPermissions";
    }

    public static class Settings
    {
        [PermissionDefinition("Administration", "Settings", "مشاهده تنظیمات", "امکان مشاهده تنظیمات سیستم")]
        public const string View = "Settings.View";

        [PermissionDefinition("Administration", "Settings", "ویرایش تنظیمات", "امکان ویرایش تنظیمات سیستم")]
        public const string Edit = "Settings.Edit";
    }

    public static class Test
    {
        [PermissionDefinition("Test", "Test", "نمایش فرم تست", "امکان مشاهده فرم تست")]
        public const string View = "Test.View";

        [PermissionDefinition("Test", "Test", "نمایش فرم تست دوم", "امکان مشاهده فرم تست دوم")]
        public const string View2 = "Test.View2";

        [PermissionDefinition("Test3", "Test3", "نمایش فرم تست سوم", "امکان مشاهده فرم تست سوم")]
        public const string View3 = "Test.View3";
    }
}