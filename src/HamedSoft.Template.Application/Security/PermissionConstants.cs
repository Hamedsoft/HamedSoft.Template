namespace HamedSoft.Template.Application.Security;

[PermissionModule("مدیر سیستم")]
public static class PermissionConstants
{
    [PermissionModule("مدیر سیستم")]
    [PermissionCategory("کاربران")]
    public static class Users
    {
        [PermissionDefinition("مشاهده لیست کاربران", "کاربر با این امکان می تواند لیست کاربران را مشاهده کند")]
        public const string View = nameof(Users) + "." + nameof(View);

        [PermissionDefinition("ایجاد کاربر", "امکان ایجاد کاربر جدید")]
        public const string Create = nameof(Users) + "." + nameof(Create);

        [PermissionDefinition("ویرایش کاربر", "امکان ویرایش اطلاعات کاربر")]
        public const string Edit = nameof(Users) + "." + nameof(Edit);

        [PermissionDefinition("حذف کاربر", "امکان حذف کاربر")]
        public const string Delete = nameof(Users) + "." + nameof(Delete);
    }

    [PermissionModule("مدیر سیستم")]
    [PermissionCategory("نقش‌ها")]
    public static class Roles
    {
        [PermissionDefinition("مشاهده لیست نقش‌ها", "امکان مشاهده فهرست نقش‌ها")]
        public const string View = nameof(Roles) + "." + nameof(View);

        [PermissionDefinition("ایجاد نقش", "امکان ایجاد نقش جدید")]
        public const string Create = nameof(Roles) + "." + nameof(Create);

        [PermissionDefinition("ویرایش نقش", "امکان ویرایش نقش")]
        public const string Edit = nameof(Roles) + "." + nameof(Edit);

        [PermissionDefinition("حذف نقش", "امکان حذف نقش")]
        public const string Delete = nameof(Roles) + "." + nameof(Delete);

        [PermissionDefinition("مدیریت دسترسی‌های نقش", "امکان تعیین دسترسی‌های یک نقش")]
        public const string AssignPermissions = nameof(Roles) + "." + nameof(AssignPermissions);
    }

    [PermissionModule("تنظیمات")]
    [PermissionCategory("تنظیمات")]
    public static class Settings
    {
        [PermissionDefinition("مشاهده تنظیمات", "امکان مشاهده تنظیمات سیستم")]
        public const string View = nameof(Settings) + "." + nameof(View);

        [PermissionDefinition("ویرایش تنظیمات", "امکان ویرایش تنظیمات سیستم")]
        public const string Edit = nameof(Settings) + "." + nameof(Edit);
    }

    [PermissionModule("ماژول تست")]
    [PermissionCategory("تست")]
    public static class Test
    {
        [PermissionDefinition("تست 1", "تست 1 1")]
        public const string View = nameof(Test) + "." + nameof(View);

        [PermissionDefinition("تست 2", "تست 2 2")]
        public const string View2 = nameof(Test) + "." + nameof(View2);
    }
}