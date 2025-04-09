using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.Shared;
using ReadTrack.Shared.Requests;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Controllers;

public class SessionControllerTests : BaseControllerTests
{
    [Fact]
    public async Task CanGetSessionCount()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();        
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(m => m.GetSessionCountAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var bookId = RandomGenerator.CreateNumber(1, 99);
        var response = await controller.GetSessionCountAsync(bookId);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        sessionServiceMock.Verify(m => m.GetSessionCountAsync(user.Id, bookId), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanGetFirstTenSessions()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(m => m.GetSessionsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Session>());

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var bookId = RandomGenerator.CreateNumber(1, 99);
        var offset = RandomGenerator.CreateNumber(1, 99);
        var count = RandomGenerator.CreateNumber(1, 99);
        var response = await controller.GetSessionsAsync(bookId, offset, count);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        sessionServiceMock.Verify(m => m.GetSessionsAsync(user.Id, bookId, offset, count), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanCreateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(m => m.CreateSessionAsync(It.IsAny<int>(), It.IsAny<CreateSessionRequest>()))
            .ReturnsAsync(new Session());

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var request = RandomGenerator.GenerateRandomCreateSessionRequest();
        var response = await controller.CreateSessionAsync(request);

        // Assert
        response.Should().BeOfType<CreatedResult>();

        sessionServiceMock.Verify(m => m.CreateSessionAsync(user.Id, request), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);    
    }

    [Fact]
    public async Task CanUpdateSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(m => m.UpdateSessionAsync(It.IsAny<int>(), It.IsAny<Session>()))
            .ReturnsAsync(true);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var session = RandomGenerator.GenerateRandomSessionModel();        
        var response = await controller.UpdateSessionAsync(session.Id, session);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        sessionServiceMock.Verify(m => m.UpdateSessionAsync(user.Id, session), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CanDeleteSession()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(m => m.DeleteSessionAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(true);

        var controller = new SessionController(new Mock<ILogger<SessionController>>().Object, userServiceMock.Object, sessionServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var sessionId = RandomGenerator.CreateNumber(1, 99);
        var response = await controller.DeleteSessionAsync(sessionId);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        sessionServiceMock.Verify(m => m.DeleteSessionAsync(user.Id, sessionId), Times.Once);
        userServiceMock.Verify(m => m.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
    }
}