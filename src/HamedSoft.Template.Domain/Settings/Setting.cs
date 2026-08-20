using HamedSoft.Template.Domain.SeedWork;

namespace HamedSoft.Template.Domain.Settings;

/// <summary>
/// Represents an application setting and its definition.
/// </summary>
public sealed class Setting : AggregateRoot<Guid>
{
    private Setting()
    {
    }

    private Setting(
        Guid id,
        string key,
        string module,
        string feature,
        string category,
        string value,
        SettingValueType valueType,
        string? defaultValue,
        bool isRequired,
        bool isSensitive,
        bool isSecret,
        string? description)
        : base(id)
    {
        SetDefinition(
            key,
            module,
            feature,
            category,
            valueType,
            defaultValue,
            isRequired,
            isSensitive,
            isSecret,
            description);

        ChangeValue(value);
    }

    /// <summary>
    /// Gets the unique setting key.
    /// </summary>
    public string Key { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the module that owns the setting.
    /// </summary>
    public string Module { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the feature that owns the setting.
    /// </summary>
    public string Feature { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the category of the setting.
    /// </summary>
    public string Category { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the stored setting value.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the type of the setting value.
    /// </summary>
    public SettingValueType ValueType { get; private set; }

    /// <summary>
    /// Gets the default value of the setting.
    /// </summary>
    public string? DefaultValue { get; private set; }

    /// <summary>
    /// Gets whether the setting must have a value.
    /// </summary>
    public bool IsRequired { get; private set; }

    /// <summary>
    /// Gets whether the setting contains sensitive information.
    /// </summary>
    public bool IsSensitive { get; private set; }

    /// <summary>
    /// Gets the description of the setting.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets whether the setting contains Password information.
    /// </summary>
    public bool IsSecret { get; private set; }

    /// <summary>
    /// Creates a new setting.
    /// </summary>
    public static Setting Create(
        string key,
        string module,
        string feature,
        string category,
        string value,
        SettingValueType valueType,
        string? defaultValue = null,
        bool isRequired = false,
        bool isSensitive = false,
        bool isSecret = false,
        string? description = null)
    {
        return new Setting(
            Guid.NewGuid(),
            key,
            module,
            feature,
            category,
            value,
            valueType,
            defaultValue,
            isRequired,
            isSensitive,
            isSecret,
            description);
    }

    /// <summary>
    /// Changes the current setting value.
    /// </summary>
    public void ChangeValue(string value)
    {
        Guard.AgainstNull(value, nameof(value));

        if (IsRequired)
        {
            Guard.AgainstNullOrWhiteSpace(value, nameof(value));
        }

        Value = value;
    }

    private void SetDefinition(
        string key,
        string module,
        string feature,
        string category,
        SettingValueType valueType,
        string? defaultValue,
        bool isRequired,
        bool isSensitive,
        bool isSecret,
        string? description)
    {
        Guard.AgainstNullOrWhiteSpace(key, nameof(key));
        Guard.AgainstNullOrWhiteSpace(module, nameof(module));
        Guard.AgainstNullOrWhiteSpace(feature, nameof(feature));
        Guard.AgainstNullOrWhiteSpace(category, nameof(category));

        Key = key;
        Module = module;
        Feature = feature;
        Category = category;
        ValueType = valueType;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
        IsSensitive = isSensitive;
        IsSecret = isSecret;
        Description = description;
    }
}