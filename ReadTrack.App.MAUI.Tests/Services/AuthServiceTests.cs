using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Tests.Utilities;
using ReadTrack.Shared.Api;
using ReadTrack.Shared.Models.Requests;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task AuthService_LoginAsync_SavesTokenAndUserInfoWithValidLogin()
    {
        // Arrange
        var tokenResponse = RandomGenerator.GenerateRandomTokenResponse();

        var apiMock = new Mock<IAuthApi>();
        apiMock.Setup(m => m.LoginAsync(It.IsAny<AuthRequest>())).ReturnsAsync(tokenResponse);

        var userServiceMock = new Mock<IUserService>();

        var service = new AuthService(new Mock<ILogger<AuthService>>().Object, apiMock.Object, userServiceMock.Object);

        // Act
        var request = RandomGenerator.GenerateRandomAuthRequest();
        var result = await service.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(tokenResponse);

        apiMock.Verify(m => m.LoginAsync(request), Times.Once);
        apiMock.VerifyNoOtherCalls();

        userServiceMock.Verify(m => m.SetTokenInfoAsync(tokenResponse), Times.Once);
        userServiceMock.Verify(m => m.SetUserInfoAsync(tokenResponse.User!), Times.Once);
        userServiceMock.VerifyNoOtherCalls();
    }
}