using ReadTrack.Shared.Models.Analytics;
using Refit;

namespace ReadTrack.Shared.Api;

public interface IAnalyticsApi
{
    [Get("/api/analytics/books/{start}/{end}")]
    Task<IEnumerable<TimeSegment<int>>> GetBooksReadAsync(DateTime start, DateTime end);


    [Get("/api/analytics/time/{start}/{end}")]
    Task<IEnumerable<TimeSegment<int>>> GetReadingTimeAsync(DateTime start, DateTime end);
}