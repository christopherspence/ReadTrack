using ReadTrack.Shared.Models.Analytics;

namespace ReadTrack.API.Services;

public interface IAnalyticsService : IService
{
    Task<IEnumerable<TimeSegment<int>>> GetBooksReadAsync(int userId, DateTime start, DateTime end);
    Task<IEnumerable<TimeSegment<int>>> GetReadingTimeAsync(int userId, DateTime start, DateTime end);
}