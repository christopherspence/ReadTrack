using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Data.Entities;
using ReadTrack.Shared;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Services;

public class SessionServiceTests : BaseServiceTests
{
    [Fact]
    public async Task CanGetSessionCount()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(sessions);
        await Context.SaveChangesAsync();
    
        var service = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetSessionCountAsync(user.Id, book.Id);

        // Assert
        result.Should().Be(100);
    }

    [Fact]
    public async Task CanGetFirstTenSessions()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(sessions);
        await Context.SaveChangesAsync();

        var service = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, Mapper);

        // Act
        var result = await service.GetSessionsAsync(user.Id, book.Id, 0, 10);

        // Assert
        result.Should().BeEquivalentTo(Mapper.Map<IEnumerable<SessionEntity>, IEnumerable<Session>>(sessions.Take(10)));
    }

    [Fact]
    public async Task CanCreateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();        
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var service = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, Mapper);

        // Act
        var request = RandomGenerator.GenerateRandomCreateSessionRequest(book.Id);
        var result = await service.CreateSessionAsync(user.Id, request);

        // Assert
        var session = new SessionEntity
        {
            Id = 1,
            BookId = request.BookId,
            NumberOfPages = request.NumberOfPages,
            Date = request.Date,
            Duration = request.Duration,
            StartPage = request.StartPage,
            EndPage = request.EndPage,
            UserId = user.Id
        };

        result.Should().BeEquivalentTo(Mapper.Map<SessionEntity, Session>(session));

        (await Context.Sessions.SingleAsync()).Should().BeEquivalentTo(session, o => o.Excluding(n =>
            n.Path.EndsWith("Book") ||
            n.Path.EndsWith("Created") ||
            n.Path.EndsWith("Modified")));
    }

    [Fact]
    public async Task CanUpdateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(sessions);
        await Context.SaveChangesAsync();

        var service = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, Mapper);

        // Act
        var firstSession = Mapper.Map<SessionEntity, Session>(sessions.First());
        firstSession.StartPage = RandomGenerator.CreateNumber(1, 1000);
        firstSession.EndPage = RandomGenerator.CreateNumber(1, 1000);
        firstSession.Date = DateTime.UtcNow;
        firstSession.Duration = TimeSpan.FromTicks(RandomGenerator.CreateNumber(1, 100000));
        firstSession.NumberOfPages = RandomGenerator.CreateNumber(1, 100);

        var result = await service.UpdateSessionAsync(user.Id, firstSession);

        // Assert
        result.Should().BeTrue();
        
        var expected = Mapper.Map<Session, SessionEntity>(firstSession);
        expected.BookId = book.Id;        

        (await Context.Sessions.FirstAsync()).Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.EndsWith("Date") ||
                                  n.Path.EndsWith("Book") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));
    }

    [Fact]
    public async Task CanDeleteSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(sessions);
        await Context.SaveChangesAsync();

        var service = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, Mapper);

        // Act
        var result = await service.DeleteSessionAsync(user.Id, sessions.First().Id);

        // Assert
        result.Should().BeTrue();

        (await Context.Sessions.FirstAsync()).IsDeleted.Should().BeTrue();
    }
}