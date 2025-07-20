using System.Globalization;

namespace DevSandbox.Shared.Kernel.Extensions;

public static class DateTimeExtensions
{
    // For DateTime
    public static string ToShamsi(this DateTime date, string format = "yyyy/MM/dd")
    {
        var pc = new PersianCalendar();
        int year = pc.GetYear(date);
        int month = pc.GetMonth(date);
        int day = pc.GetDayOfMonth(date);
        int hour = pc.GetHour(date);
        int minute = pc.GetMinute(date);
        int second = pc.GetSecond(date);

        if (format == "yyyy/MM/dd")
            return $"{year:0000}/{month:00}/{day:00}";

        if (format == "yyyy/MM/dd HH:mm")
            return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}";

        if (format == "yyyy/MM/dd HH:mm:ss")
            return $"{year:0000}/{month:00}/{day:00} {hour:00}:{minute:00}:{second:00}";

        return $"{year:0000}/{month:00}/{day:00}";
    }

    // For DateTime (nullable)
    public static string ToShamsi(this DateTime? date, string format = "yyyy/MM/dd")
    {
        return date.HasValue ? date.Value.ToShamsi(format) : "";
    }

    // For DateOnly
    public static string ToShamsi(this DateOnly date)
    {
        var dt = date.ToDateTime(TimeOnly.MinValue);
        var pc = new PersianCalendar();
        int year = pc.GetYear(dt);
        int month = pc.GetMonth(dt);
        int day = pc.GetDayOfMonth(dt);
        return $"{year:0000}/{month:00}/{day:00}";
    }

    // For DateOnly (nullable)
    public static string ToShamsi(this DateOnly? date)
    {
        if (date == null) return "";
        return date.Value.ToShamsi();
    }
}
