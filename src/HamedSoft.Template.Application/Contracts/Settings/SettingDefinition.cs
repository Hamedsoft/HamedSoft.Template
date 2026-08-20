using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Application.Contracts.Settings;

/// <summary>
/// Represents a developer-defined application setting.
/// </summary>
public sealed record SettingDefinition(
    string Key,
    string Module,
    string Feature,
    string Category,
    string? Value,
    SettingValueType ValueType,
    string? DefaultValue,
    bool IsRequired,
    bool IsSensitive,
    bool IsSecret,
    string? Description);