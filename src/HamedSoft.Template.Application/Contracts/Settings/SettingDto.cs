namespace HamedSoft.Template.Application.Contracts.Settings;

/// <summary>
/// Represents a setting exposed to the application layer.
/// </summary>
public sealed record SettingDto(
    Guid Id,
    string Key,
    string Module,
    string Feature,
    string Category,
    string Value,
    string? DefaultValue,
    int ValueType,
    bool IsRequired,
    bool IsSensitive,
    bool IsSecret,
    string? Description);