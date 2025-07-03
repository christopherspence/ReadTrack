using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.Shared;
using ReadTrack.Shared.Models.Requests;
using ReadTrack.Web.Blazor.Api;
using ReadTrack.Web.Blazor.Services;
using ReadTrack.Web.Blazor.Tests.Utilities;

namespace ReadTrack.Web.Blazor.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task AuthService_Login_LogsUserIn()
    {
        // Arrange
        var response = RandomGenerator.GenerateRandomTokenResponse();

        var authApiMock = new Mock<IAuthApi>();
        authApiMock.Setup(m => m.LoginAsync(It.IsAny<AuthRequest>())).ReturnsAsync(response);

        var userServiceMock = new Mock<IUserService>();
        
        var service = new AuthService(
            new Mock<ILogger<AuthService>>().Object,
            authApiMock.Object,
            userServiceMock.Object);

        // Act
        var email = RandomGenerator.CreateEmailAddress();
        var password = RandomGenerator.CreatePlaceName();
        var result = await service.LoginAsync(email, password);
        
        // Assert
        result.Should().BeTrue();

        authApiMock.Verify(m => m.LoginAsync(It.Is<AuthRequest>(
            r => r.Email == email && r.Password == password)), Times.Once);

        userServiceMock.Verify(m => m.SetTokenInfoAsync(response), Times.Once);
        userServiceMock.Verify(m => m.SetUserInfoAsync(response.User!), Times.Once);
    }

    [Fact]
    public async Task AuthService_Login_ShouldLogErrorIfUserIsMissing()
    {
        // Arrange
        var response = RandomGenerator.GenerateRandomTokenResponse(false);

        var authApiMock = new Mock<IAuthApi>();
        authApiMock.Setup(m => m.LoginAsync(It.IsAny<AuthRequest>())).ReturnsAsync(response);

        var userServiceMock = new Mock<IUserService>();
        
        var loggerMock = new Mock<ILogger<AuthService>>();

        var service = new AuthService(
            loggerMock.Object,
            authApiMock.Object,
            userServiceMock.Object);

        // Act
        var email = RandomGenerator.CreateEmailAddress();
        var password = RandomGenerator.CreatePlaceName();
        var result = await service.LoginAsync(email, password);
        
        // Assert
        result.Should().BeFalse();

        loggerMock.VerifyLog(l => l.LogError("User missing in server response"), Times.Once);

        authApiMock.Verify(m => m.LoginAsync(It.Is<AuthRequest>(
            r => r.Email == email && r.Password == password)), Times.Once);

        // Should not get to this point
        userServiceMock.Verify(m => m.SetTokenInfoAsync(It.IsAny<TokenResponse>()), Times.Never);
        userServiceMock.Verify(m => m.SetUserInfoAsync(It.IsAny<User>()), Times.Never);
    }
}