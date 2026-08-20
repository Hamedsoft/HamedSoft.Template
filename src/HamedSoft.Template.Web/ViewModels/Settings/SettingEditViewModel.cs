using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Web.ViewModels.Settings;

public sealed class SettingEditViewModel
{
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Module { get; set; } = string.Empty;

    public string Feature { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public SettingValueType ValueType { get; set; }

    public string? DefaultValue { get; set; }

    public bool IsRequired { get; set; }

    public bool IsSensitive { get; set; }

    public bool IsSecret { get; set; }

    public string? Description { get; set; }
}