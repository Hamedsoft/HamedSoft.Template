using System.Globalization;
using System.Text.Json;
using HamedSoft.Template.Domain.Settings;

namespace HamedSoft.Template.Web.Formatting;

/// <summary>
/// Normalizes setting values received from the UI into canonical
/// representations suitable for persistence.
/// </summary>
public static class SettingValueNormalizer
{
    /// <summary>
    /// Attempts to normalize a setting value according to its declared type.
    /// </summary>
    public static bool TryNormalize(
        SettingValueType valueType,
        string? value,
        out string normalizedValue,
        out string? error)
    {
        normalizedValue = string.Empty;
        error = null;

        value ??= string.Empty;

        switch (valueType)
        {
            case SettingValueType.String:
                normalizedValue = value;
                return true;

            case SettingValueType.Int:
                if (int.TryParse(
                    value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var intValue))
                {
                    normalizedValue = intValue.ToString(
                        CultureInfo.InvariantCulture);

                    return true;
                }

                error = "مقدار وارد شده باید یک عدد صحیح معتبر باشد.";
                return false;

            case SettingValueType.Long:
                if (long.TryParse(
                    value,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var longValue))
                {
                    normalizedValue = longValue.ToString(
                        CultureInfo.InvariantCulture);

                    return true;
                }

                error = "مقدار وارد شده باید یک عدد صحیح معتبر باشد.";
                return false;

            case SettingValueType.Decimal:
                if (decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var decimalValue))
                {
                    normalizedValue = decimalValue.ToString(
                        CultureInfo.InvariantCulture);

                    return true;
                }

                error = "مقدار وارد شده باید یک عدد اعشاری معتبر باشد.";
                return false;

            case SettingValueType.Boolean:
                if (bool.TryParse(value, out var boolValue))
                {
                    normalizedValue = boolValue
                        ? bool.TrueString
                        : bool.FalseString;

                    return true;
                }

                error = "مقدار Boolean نامعتبر است.";
                return false;

            case SettingValueType.DateTime:
                return TryNormalizeDateTime(
                    value,
                    out normalizedValue,
                    out error);

            case SettingValueType.TimeSpan:
                return TryNormalizeTimeSpan(
                    value,
                    out normalizedValue,
                    out error);

            case SettingValueType.Json:
                return TryNormalizeJson(
                    value,
                    out normalizedValue,
                    out error);

            default:
                error = "نوع Setting پشتیبانی نمی‌شود.";
                return false;
        }
    }

    private static bool TryNormalizeDateTime(
        string value,
        out string normalizedValue,
        out string? error)
    {
        normalizedValue = string.Empty;
        error = null;

        if (!DateTime.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out var dateTime))
        {
            error = "مقدار DateTime نامعتبر است.";
            return false;
        }

        normalizedValue = dateTime.ToString(
            "O",
            CultureInfo.InvariantCulture);

        return true;
    }

    private static bool TryNormalizeTimeSpan(
        string value,
        out string normalizedValue,
        out string? error)
    {
        normalizedValue = string.Empty;
        error = null;

        if (!TimeSpan.TryParse(
            value,
            CultureInfo.InvariantCulture,
            out var timeSpan))
        {
            error = "مقدار TimeSpan نامعتبر است.";
            return false;
        }

        normalizedValue = timeSpan.ToString(
            "c",
            CultureInfo.InvariantCulture);

        return true;
    }

    private static bool TryNormalizeJson(
        string value,
        out string normalizedValue,
        out string? error)
    {
        normalizedValue = string.Empty;
        error = null;

        if (string.IsNullOrWhiteSpace(value))
        {
            normalizedValue = value;
            return true;
        }

        try
        {
            using var document = JsonDocument.Parse(value);

            normalizedValue = document.RootElement.GetRawText();

            return true;
        }
        catch (JsonException)
        {
            error = "مقدار JSON نامعتبر است.";
            return false;
        }
    }
}