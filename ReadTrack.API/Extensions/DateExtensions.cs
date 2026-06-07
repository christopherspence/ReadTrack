using ReadTrack.Shared.Models.Analytics;

namespace ReadTrack.API.Extensions;

public static class DateExtensions
{
    public static DateTime StartOfDay(this DateTime date) => new(date.Year, date.Month, date.Day, 0, 0, 0);

    public static DateTime EndOfDay(this DateTime date) => new(date.Year, date.Month, date.Day, 23, 59, 59);

    public static int TotalDays(DateTime start, DateTime end) => (int)Math.Abs((end - start).TotalDays);
}