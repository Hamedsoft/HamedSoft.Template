using System.Globalization;

namespace HamedSoft.Template.Web.Formatting;

/// <summary>
/// Formats TimeSpan values for Persian UI presentation.
/// </summary>
public static class PersianTimeSpanFormatter
{
    /// <summary>
    /// Formats a TimeSpan using a 12-hour Persian representation.
    /// </summary>
    public static string ToTime(
        TimeSpan value,
        bool includeSeconds = true)
    {
        if (value.TotalDays >= 1)
        {
            value = value.Subtract(
                TimeSpan.FromDays(
                    Math.Floor(value.TotalDays)));
        }

        var hour = value.Hours;

        var meridiem = hour switch
        {
            0 => "بامداد",
            < 12 => "صبح",
            12 => "ظهر",
            < 18 => "عصر",
            _ => "شب"
        };

        var displayHour = hour switch
        {
            0 => 12,
            > 12 => hour - 12,
            _ => hour
        };

        var time = includeSeconds
            ? string.Create(
                CultureInfo.InvariantCulture,
                $"{displayHour:00}:{value.Minutes:00}:{value.Seconds:00}")
            : string.Create(
                CultureInfo.InvariantCulture,
                $"{displayHour:00}:{value.Minutes:00}");

        return PersianDateTimeFormatter.ConvertToPersianNumbers(
            $"{time} {meridiem}");
    }
}