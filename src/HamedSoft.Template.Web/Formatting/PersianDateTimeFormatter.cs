using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public static string ToPersianDate(DateTime value)
    {
        var date = string.Create(CultureInfo.InvariantCulture, $"{Calendar.GetYear(value):0000}/{Calendar.GetMonth(value):00}/{Calendar.GetDayOfMonth(value):00}");
        return $"{ConvertToPersianNumbers(date)}";
    }
    public static string ToTime(DateTime value, bool includeSeconds = true)
    {
        var time = includeSeconds
            ? value.ToString("HH:mm:ss", CultureInfo.InvariantCulture)
            : value.ToString("HH:mm", CultureInfo.InvariantCulture);

        return $"{time}";
    }
    public static string? ConvertToPersianNumbers(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return value
            .Replace('0', '۰')
            .Replace('1', '۱')
            .Replace('2', '۲')
            .Replace('3', '۳')
            .Replace('4', '۴')
            .Replace('5', '۵')
            .Replace('6', '۶')
            .Replace('7', '۷')
            .Replace('8', '۸')
            .Replace('9', '۹');
    }
}