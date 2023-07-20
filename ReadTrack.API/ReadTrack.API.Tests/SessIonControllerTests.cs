using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Data.Entities;
using ReadTrack.API.Models;
using ReadTrack.API.Models.Requests;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests;

public class SessionControllerTests : BaseTests
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
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(book);

        var sessionService = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, bookServiceMock.Object, Mapper);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetSessionCountAsync(book.Id);

        // Assert
        var result = (int)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        result.Should().Be(100);

        bookServiceMock.Verify(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>()));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanGetFirstTenSessions()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var Sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(Sessions);
        await Context.SaveChangesAsync();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(book);

        var sessionService = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, bookServiceMock.Object, Mapper);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetSessionsAsync(book.Id, 0, 10);

        // Assert
        var result = (IEnumerable<Session>)response.Should().BeOfType<OkObjectResult>().Subject.Value;

        result.Should().BeEquivalentTo(Mapper.Map<IEnumerable<SessionEntity>, IEnumerable<Session>>(Sessions.Take(10)));

        bookServiceMock.Verify(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>()));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanCreateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var session = RandomGenerator.GenerateRandomSession();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));


        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(book);

        var sessionService = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, bookServiceMock.Object, Mapper);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.CreateSessionAsync(new CreateSessionRequest
        {
            BookId = book.Id,
            StartPage = session.StartPage,
            EndPage = session.EndPage,
            NumberOfPages = session.NumberOfPages,
            Date = session.Date,
            Duration = session.Duration
        });

        // Assert
        var result = (Session)response.Should().BeOfType<CreatedResult>().Subject.Value;

        result.Should().BeEquivalentTo(Mapper.Map<SessionEntity, Session>(session));

        (await Context.Sessions.SingleAsync()).Should().BeEquivalentTo(session, o => o.Excluding(n =>
            n.Path.EndsWith("Book") ||
            n.Path.EndsWith("Created") ||
            n.Path.EndsWith("Modified")));

        bookServiceMock.Verify(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>()));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);    
    }

    [Fact]
    public async Task CanUpdateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();
        var book = Mapper.Map<BookEntity, Book>(RandomGenerator.GenerateRandomBook());

        var Sessions = RandomGenerator.GenerateOneHundredRandomSessions();
        await Context.Sessions.AddRangeAsync(Sessions);
        await Context.SaveChangesAsync();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(book);

        var sessionService = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, bookServiceMock.Object, Mapper);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var firstSession = Mapper.Map<SessionEntity, Session>(Sessions.First());
        firstSession.StartPage = RandomGenerator.CreateNumber(1, 1000);
        firstSession.EndPage = RandomGenerator.CreateNumber(1, 1000);
        firstSession.Date = DateTime.UtcNow;
        firstSession.Duration = TimeSpan.FromTicks(RandomGenerator.CreateNumber(1, 100000));
        firstSession.NumberOfPages = RandomGenerator.CreateNumber(1, 100);

        var response = await controller.UpdateSessionAsync(1, firstSession);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        var expected = Mapper.Map<Session, SessionEntity>(firstSession);
        expected.BookId = book.Id;        

        (await Context.Sessions.FirstAsync()).Should().BeEquivalentTo(expected,
            o => o.Excluding(n => n.Path.EndsWith("Date") ||
                                  n.Path.EndsWith("Book") ||
                                  n.Path.EndsWith("Created") ||
                                  n.Path.EndsWith("Modified")));

        bookServiceMock.Verify(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>()));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
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
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Mapper.Map<UserEntity, User>(user));

        var bookServiceMock = new Mock<IBookService>();
        bookServiceMock.Setup(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(book);

        var sessionService = new SessionService(new Mock<ILogger<SessionService>>().Object, Context, bookServiceMock.Object, Mapper);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionService)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.DeleteSessionAsync(sessions.First().Id);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        (await Context.Sessions.FirstAsync()).IsDeleted.Should().BeTrue();

        bookServiceMock.Verify(m => m.GetBookAsync(It.IsAny<int>(), It.IsAny<int>()));
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }
}