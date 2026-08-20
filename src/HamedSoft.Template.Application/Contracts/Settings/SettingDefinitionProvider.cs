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
                Key: "Treasury.ElectronicTransfer.System.Username",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: "demo-user",
                ValueType: SettingValueType.String,
                DefaultValue: "demo-user",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Electronic transfer system username."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.Password",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: "مقدار",
                ValueType: SettingValueType.String,
                DefaultValue: "demo-password",
                IsRequired: true,
                IsSensitive: true,
                IsSecret: true,
                Description: "Electronic transfer system password."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.Timeout",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: "30000",
                ValueType: SettingValueType.Long,
                DefaultValue: "30000",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Electronic transfer request timeout in milliseconds."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.Enabled",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: "true",
                ValueType: SettingValueType.Boolean,
                DefaultValue: "true",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Enables electronic transfer integration."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.MaxRetryCount",
                Module: "Treasury",
                Feature: "System",
                Category: "System",
                Value: "3",
                ValueType: SettingValueType.Int,
                DefaultValue: "3",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Maximum number of retry attempts."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.AmountLimit",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: "1000000.50",
                ValueType: SettingValueType.Decimal,
                DefaultValue: "1000000.50",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Maximum allowed transfer amount."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.RequestDate",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value : "2026-08-20T12:00:00", 
                ValueType: SettingValueType.DateTime,
                DefaultValue: "2026-08-20T12:00:00",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Default request date."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.RequestTimeout",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value : "00:05:00", ValueType: SettingValueType.TimeSpan,
                DefaultValue: "00:05:00",
                IsRequired: true,
                IsSensitive: false,
                IsSecret: false,
                Description: "Electronic transfer request timeout."),

            new SettingDefinition(
                Key: "Treasury.ElectronicTransfer.System.AdvancedOptions",
                Module: "Treasury",
                Feature: "ElectronicTransfer",
                Category: "System",
                Value: """{"endpoint":"https://example.test/api","retryEnabled":true}""",
                ValueType: SettingValueType.Json,
                DefaultValue: """{"endpoint":"https://example.test/api","retryEnabled":true}""",
                IsRequired: false,
                IsSensitive: false,
                IsSecret: false,
                Description: "Advanced electronic transfer options.")
        ];
    }
}