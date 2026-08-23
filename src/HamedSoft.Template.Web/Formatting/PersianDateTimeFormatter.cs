using System.Globalization;

namespace HamedSoft.Template.Web.Formatting;

/// <summary>
/// Formats Gregorian DateTime values for Persian UI presentation.
/// </summary>
public static class PersianDateTimeFormatter
{
    private static readonly PersianCalendar Calendar = new();

    public static string ToDateTime(DateTime value, bool includeSeconds = true)
    {
        var date = string.Create(CultureInfo.InvariantCulture, $"{Calendar.GetYear(value):0000}/{Calendar.GetMonth(value):00}/{Calendar.GetDayOfMonth(value):00}");
        var time = includeSeconds ? value.ToString("HH:mm:ss", CultureInfo.InvariantCulture) : value.ToString("HH:mm", CultureInfo.InvariantCulture);

        return $"{date} {time}";
    }

    public static string ToDate(DateTime value)
    {
        var date = string.Create(CultureInfo.InvariantCulture, $"{Calendar.GetYear(value):0000}/{Calendar.GetMonth(value):00}/{Calendar.GetDayOfMonth(value):00}");
        return $"{date}";
    }

    public static string ToTime(DateTime value, bool includeSeconds = true)
    {
        var time = includeSeconds
            ? value.ToString("HH:mm:ss", CultureInfo.InvariantCulture)
            : value.ToString("HH:mm", CultureInfo.InvariantCulture);

        return $"{time}";
    }
}