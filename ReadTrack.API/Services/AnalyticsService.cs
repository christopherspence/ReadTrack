using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadTrack.API.Data;
using ReadTrack.API.Extensions;
using ReadTrack.Shared.Models.Analytics;

namespace ReadTrack.API.Services;

public class AnalyticsService : BaseService<AnalyticsService>, IAnalyticsService
{
    public AnalyticsService(ILogger<AnalyticsService> logger, ReadTrackContext context, IMapper mapper) : base(logger, context, mapper)
    {
    }

    public async Task<IEnumerable<TimeSegment<int>>> GetBooksReadAsync(int userId, DateTime start, DateTime end)
    {
        var booksRead = new List<TimeSegment<int>>();

        var sessions = await Context.Sessions.Where(s => s.UserId == userId && s.Date >= start.StartOfDay() && s.Date <= end.EndOfDay() && !s.IsDeleted).ToListAsync();

        for (var i = 0; i <= DateExtensions.TotalDays(start, end); i++)
        {
            var date = start.AddDays(i);
            
            var books = sessions.Where(s => s.Date >= date.StartOfDay() && s.Date <= date.EndOfDay()).GroupBy(s => s.BookId).Count();

            booksRead.Add(new TimeSegment<int>
            {
                Date = date,
                Type = start.DetermineSegmentType(end),
                Value = books
            });
        }
        
        return booksRead;
    }

    public async Task<IEnumerable<TimeSegment<int>>> GetPagesReadAsync(int userId, DateTime start, DateTime end)
    {
        var pagesRead = new List<TimeSegment<int>>();

        var sessions = await Context.Sessions.Where(s => s.UserId == userId && s.Date >= start.StartOfDay() && s.Date <= end.EndOfDay() && !s.IsDeleted).ToListAsync();

        for (var i = 0; i <= DateExtensions.TotalDays(start, end); i++)
        {
            var date = start.AddDays(i);
            
            var pages = sessions.Where(s => s.Date >= date.StartOfDay() && s.Date <= date.EndOfDay()).Sum(s => s.NumberOfPages);

            pagesRead.Add(new TimeSegment<int>
            {
                Date = date,
                Type = start.DetermineSegmentType(end),
                Value = pages ?? 0
            });
        }
        
        return pagesRead;
    }

    public async Task<IEnumerable<TimeSegment<int>>> GetReadingTimeAsync(int userId, DateTime start, DateTime end)
    {
        var readingTime = new List<TimeSegment<int>>();

        var sessions = await Context.Sessions.Where(s => s.UserId == userId && s.Date >= start.StartOfDay() && s.Date <= end.EndOfDay() && !s.IsDeleted).ToListAsync();

        for (var i = 0; i <= DateExtensions.TotalDays(start, end); i++)
        {
            var date = start.AddDays(i);
            
            var time = sessions.Where(s => s.Date >= date.StartOfDay() && s.Date <= date.EndOfDay()).Sum(s => (int)s.Duration.TotalMinutes);

            readingTime.Add(new TimeSegment<int>
            {
                Date = date,
                Type = start.DetermineSegmentType(end),
                Value = time
            });
        }
        
        return readingTime;
    }
}