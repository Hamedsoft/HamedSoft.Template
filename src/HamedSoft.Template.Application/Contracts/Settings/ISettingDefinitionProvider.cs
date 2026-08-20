namespace HamedSoft.Template.Application.Contracts.Settings;

/// <summary>
/// Provides developer-defined application setting definitions.
/// </summary>
public interface ISettingDefinitionProvider
{
    IReadOnlyCollection<SettingDefinition> GetDefinitions();
}