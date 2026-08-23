namespace HamedSoft.Template.Web.Models.Settings;

/// <summary>
/// Represents the settings section rendered inside a feature page.
/// </summary>
public sealed class SettingSectionViewModel
{
    public string Module { get; init; } = string.Empty;

    public string Feature { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public IReadOnlyCollection<SettingItemViewModel> Settings { get; init; }
        = [];
}