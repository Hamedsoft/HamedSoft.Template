using HamedSoft.Template.Application.Contracts.Settings;

namespace HamedSoft.Template.Web.ViewModels.Settings;

public sealed class SettingsIndexViewModel
{
    public IReadOnlyCollection<SettingDto> Settings { get; init; }
        = Array.Empty<SettingDto>();
}