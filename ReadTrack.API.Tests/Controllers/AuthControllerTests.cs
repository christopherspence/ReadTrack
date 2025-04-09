using System;
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
using System.Threading.Tasks;

namespace ReadTrack.API.Tests.Controllers;

public class AuthControllerTests : BaseControllerTests
{
    [Fact]
    public async Task CanLogin()
    {
        // Arrange
        var authServiceMock = new Mock<IAuthService>();
        authServiceMock.Setup(m => m.LoginAsync(It.IsAny<AuthRequest>())).ReturnsAsync(new TokenResponse());
        
        var controller = new AuthController(new Mock<ILogger<AuthController>>().Object, authServiceMock.Object);

        // Act
        var request = new AuthRequest
        {
            Email = RandomGenerator.CreateEmailAddress(),
            Password = Guid.NewGuid().ToString()
        };
        
        var response = await controller.LoginAsync(request);

        // Assert
        response.Should().BeOfType<CreatedResult>();

        authServiceMock.Verify(m => m.LoginAsync(request), Times.Once);
    }
}