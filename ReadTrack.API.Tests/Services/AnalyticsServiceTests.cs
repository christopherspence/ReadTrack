using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Extensions;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using ReadTrack.Shared.Models.Analytics;
using Xunit;

namespace ReadTrack.API.Tests.Services;

public class AnalyticsServiceTests : BaseServiceTests
{
    [Fact]
    public async Task CanGetBooksRead()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var start = now.StartOfDay();
        var end = now.EndOfDay();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();

        for (var i = 0; i < sessions.Count(); i++)
        {
            sessions.ElementAt(i).BookId = books.ElementAt(i).Id;
        }

        Context.Books.AddRange(books);
        Context.Sessions.AddRange(sessions);
        await Context.SaveChangesAsync();

        var service = new AnalyticsService(new Mock<ILogger<AnalyticsService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetBooksReadAsync(1, start, end);

        result.Should().BeEquivalentTo(new List<TimeSegment<int>>
        {
            new() 
            {
                Date = now.StartOfDay(),
                Value = 100,
                Type = SegmentType.Day    
            }
        });
    }  

    [Fact]
    public async Task CanGetReadingTime()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var start = now.StartOfDay();
        var end = now.EndOfDay();

        var books = RandomGenerator.GenerateOneHundredRandomBooks();
        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();

        Context.Books.AddRange(books);
        Context.Sessions.AddRange(sessions);
        await Context.SaveChangesAsync();

        var service = new AnalyticsService(new Mock<ILogger<AnalyticsService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetReadingTimeAsync(1, start, end);

        result.Should().BeEquivalentTo(new List<TimeSegment<int>>
        {
            new() 
            {
                Date = now.StartOfDay(),
                Value = sessions.Sum(s => s.Duration.Minutes),
                Type = SegmentType.Day    
            }
        });
    }    
}