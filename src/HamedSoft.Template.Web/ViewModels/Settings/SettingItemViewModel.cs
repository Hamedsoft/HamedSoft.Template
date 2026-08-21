using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Web.Models.Settings;

/// <summary>
/// Represents a setting item for UI rendering.
/// </summary>
public sealed class SettingItemViewModel
{
    public Guid Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Value { get; init; } = string.Empty;

    public SettingValueType ValueType { get; init; }

    public string? DefaultValue { get; init; }

    public bool IsRequired { get; init; }

    public bool IsSensitive { get; init; }

    public bool IsSecret { get; init; }

    public string? Description { get; init; }

    public string DisplayValue { get; init; } = string.Empty;

    public string? InputValue { get; init; }
}