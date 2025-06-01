using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Tests.Utilities;
using ReadTrack.App.MAUI.Utilities;
using ReadTrack.Shared.Api;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task UserService_SetTokenInfoAsync_SetsTokenInfo()
    {
        // Arrange
        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IAuthApi>().Object,
            localStorageServiceMock.Object);

        // Act
        var tokenResponse = RandomGenerator.GenerateRandomTokenResponse();
        await service.SetTokenInfoAsync(tokenResponse);

        // Assert
        localStorageServiceMock.Verify(m => m.SetAsync(Constants.UserToken, tokenResponse.Token!), Times.Once);
        localStorageServiceMock.Verify(m => m.SetAsync(Constants.ExpiresAt, tokenResponse.Expires.ToString()), Times.Once);
        localStorageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UserService_SetTokenInfoAsync_SetsUserInfo()
    {
         // Arrange
        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IAuthApi>().Object,
            localStorageServiceMock.Object);

        // Act
        var user = RandomGenerator.GenerateRandomUser();
        await service.SetUserInfoAsync(user);

        // Assert
        localStorageServiceMock.Verify(m => m.SetAsync(Constants.UserId, user.Id.ToString()), Times.Once);
        localStorageServiceMock.Verify(m => m.SetAsync(Constants.UserEmail, user.Email!), Times.Once);
        localStorageServiceMock.VerifyNoOtherCalls();
    }
}