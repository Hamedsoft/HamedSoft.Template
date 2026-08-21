using System.Globalization;

namespace HamedSoft.Template.Web.Formatting;

/// <summary>
/// Formats Gregorian DateTime values for Persian UI presentation.
/// </summary>
public static class PersianDateTimeFormatter
{
    private static readonly PersianCalendar Calendar = new();

    public static string Format(
        DateTime value,
        bool includeSeconds = true)
    {
        var date = string.Create(
            CultureInfo.InvariantCulture,
            $"{Calendar.GetYear(value):0000}/{Calendar.GetMonth(value):00}/{Calendar.GetDayOfMonth(value):00}");

        var time = includeSeconds
            ? value.ToString("HH:mm:ss", CultureInfo.InvariantCulture)
            : value.ToString("HH:mm", CultureInfo.InvariantCulture);

        return $"{date} {time}";
    }

    public static string Format(
        DateTime? value,
        bool includeSeconds = true)
    {
        return value.HasValue
            ? Format(value.Value, includeSeconds)
            : string.Empty;
    }
}