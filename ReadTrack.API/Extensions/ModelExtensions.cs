using ReadTrack.Shared.Models.Analytics;

namespace ReadTrack.API.Extensions;

public static class ModelExtensions
{
    public static SegmentType DetermineSegmentType(this DateTime start, DateTime end)
    {
        var span = end - start;

        if (span.Days < 7)
        {
            return SegmentType.Day;
        }
        else if (span.Days < 30)
        {
            return SegmentType.Week;
        }
        else if (span.Days < 365)
        {
            return SegmentType.Month;
        }
        else
        {
            return SegmentType.Year;
        }
    }
}