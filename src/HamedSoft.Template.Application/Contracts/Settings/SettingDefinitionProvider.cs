using HamedSoft.Template.Application.Contracts.Settings;
using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Application.Settings;

internal sealed class SettingDefinitionProvider : ISettingDefinitionProvider
{
    public IReadOnlyCollection<SettingDefinition> GetDefinitions()
    {
        return
        [
            new SettingDefinition(
                Key: "تنظیم متنی",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: "demo-user",
                ValueType: SettingValueType.String,
                DefaultValue: "demo-user",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "این تنظیم متنی هست و برای تست هست."),

            new SettingDefinition(
                Key: "تنظیم پسوورد",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: "مقدار",
                ValueType: SettingValueType.String,
                DefaultValue: "demo-password",
                IsRequired: true,
                IsSensitive: true,
                IsSecret: true,
                Description: "Electronic transfer system password."),

            new SettingDefinition(
                Key: "تنظیم از نوع لانگ",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: "30000",
                ValueType: SettingValueType.Long,
                DefaultValue: "30000",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Electronic transfer request timeout in milliseconds."),

            new SettingDefinition(
                Key: "تنظیم از نوع بوولین",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: "true",
                ValueType: SettingValueType.Boolean,
                DefaultValue: "true",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "آیا این اتفاق فعال باشد؟"),

            new SettingDefinition(
                Key: "تنظیم عددی",
                Module: "خزانه داری",
                Feature: "صورتحساب الکترونیکی",
                Category: "صورتحساب الکترونیکی",
                Value: "3",
                ValueType: SettingValueType.Int,
                DefaultValue: "3",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "مقدارش چقدر باشد؟"),

            new SettingDefinition(
                Key: "تنظیم اعشاری",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: "1000000.50",
                ValueType: SettingValueType.Decimal,
                DefaultValue: "1000000.50",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "مقدار اعشاری چقدر باشد"),

            new SettingDefinition(
                Key: "تنظیم تاریخ",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value : "2026-08-20T12:00:00", 
                ValueType: SettingValueType.DateTime,
                DefaultValue: "2026-08-20T12:00:00",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "چندم انجام بشه. ساعتش رو هم میتونی تنظیم کنی"),

            new SettingDefinition(
                Key: "چه ساعتی انجام بشه",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value : "00:05:00", ValueType: SettingValueType.TimeSpan,
                DefaultValue: "00:05:00",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "ثانیه پیش فرض 0 هست"),

            new SettingDefinition(
                Key: "فرمت جیسان",
                Module: "خزانه داری",
                Feature: "حواله الکترونیک",
                Category: "صورتحساب الکترونیکی",
                Value: """{"endpoint":"https://example.test/api","retryEnabled":true}""",
                ValueType: SettingValueType.Json,
                DefaultValue: """{"endpoint":"https://example.test/api","retryEnabled":true}""",
                IsRequired: false,
                IsSensitive: false,
                IsSecret: false,
                Description: "برای کانفیگ سیستم میخوام جیسانشو بهم بدی")
        ];
    }
}