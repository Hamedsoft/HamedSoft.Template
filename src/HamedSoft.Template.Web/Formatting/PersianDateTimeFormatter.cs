using System.Globalization;

namespace HamedSoft.Template.Web.Formatting;

/// <summary>
/// Provides formatting utilities for Gregorian DateTime values
/// displayed in Persian UI.
/// </summary>
public static class PersianDateTimeFormatter
{
    private static readonly PersianCalendar Calendar = new();

    /// <summary>
    /// Converts a Gregorian DateTime to a Persian date.
    /// </summary>
    public static string ToPersianDate(
        DateTime value)
    {
        var date = string.Create(
            CultureInfo.InvariantCulture,
            $"{Calendar.GetYear(value):0000}/" +
            $"{Calendar.GetMonth(value):00}/" +
            $"{Calendar.GetDayOfMonth(value):00}");

        return ConvertToPersianNumbers(date);
    }

    /// <summary>
    /// Formats the time portion of a DateTime value.
    /// </summary>
    public static string ToTime(
        DateTime value,
        bool includeSeconds = true)
    {
        var hour = value.Hour;

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
            ? $"{displayHour:00}:{value.Minute:00}:{value.Second:00}"
            : $"{displayHour:00}:{value.Minute:00}";

        return ConvertToPersianNumbers(
            $"{time} {meridiem}");
    }

    /// <summary>
    /// Formats a complete Gregorian DateTime for Persian UI.
    /// </summary>
    public static string ToDateTime(
        DateTime value,
        bool includeSeconds = true)
    {
        return $"{ToPersianDate(value)} {ToTime(value, includeSeconds)}";
    }

    /// <summary>
    /// Converts Western digits to Persian digits.
    /// </summary>
    public static string ConvertToPersianNumbers(
        string value)
    {
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