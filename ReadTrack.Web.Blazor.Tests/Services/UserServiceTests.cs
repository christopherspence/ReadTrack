using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.Shared;
using ReadTrack.Web.Blazor.Api;
using ReadTrack.Web.Blazor.Services;
using ReadTrack.Web.Blazor.Tests.Utilities;

namespace ReadTrack.Web.Blazor.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task UserService_WithValidToken_IsLoggedIn()
    {
        // Arrange
        var localStorageServiceMock = new Mock<ILocalStorageService>();
        localStorageServiceMock
            .Setup(m => m.GetItemAsStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RandomGenerator.CreatePlaceName());

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IUserApi>().Object,
            localStorageServiceMock.Object);

        // Act
        var result = await service.IsLoggedInAsync();

        // Assert
        result.Should().BeTrue();
        localStorageServiceMock.Verify(m => m.GetItemAsStringAsync(Constants.UserToken, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UserService_WithValidToken_CanGetToken()
    {
        // Arrange
        var token = RandomGenerator.CreatePlaceName();

        var localStorageServiceMock = new Mock<ILocalStorageService>();
        localStorageServiceMock
            .Setup(m => m.GetItemAsStringAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IUserApi>().Object,
            localStorageServiceMock.Object);

        // Act
        var result = await service.GetTokenAsync();

        // Assert
        result.Should().Be(token);
        localStorageServiceMock.Verify(m => m.GetItemAsStringAsync(Constants.UserToken, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UserService_Register_ReturnsTokenResponse()
    {
        // Arrange
        var request = RandomGenerator.GenerateRandomCreateUserRequest();
        var response = RandomGenerator.GenerateRandomTokenResponse();

        var apiMock = new Mock<IUserApi>();
        apiMock.Setup(m => m.RegisterAsync(It.IsAny<CreateUserRequest>())).ReturnsAsync(response);

        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            apiMock.Object,
            localStorageServiceMock.Object);

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(response);
        apiMock.Verify(m => m.RegisterAsync(request), Times.Once);

        localStorageServiceMock.Verify(m => m.SetItemAsStringAsync(Constants.UserToken, response.Token!, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.Verify(m => m.SetItemAsync(Constants.ExpiresAt, response.Expires, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UserService_GetUser_GetsUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var apiMock = new Mock<IUserApi>();
        apiMock.Setup(m => m.GetUserAsync()).ReturnsAsync(user);

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            apiMock.Object,
            new Mock<ILocalStorageService>().Object);

        // Act
        var result = await service.GetUserAsync();

        // Assert
        result.Should().Be(user);
        apiMock.Verify(m => m.GetUserAsync(), Times.Once);
    }    

    [Fact]
    public async Task UserService_UpdateUser_UpdatesUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var apiMock = new Mock<IUserApi>();
        apiMock.Setup(m => m.GetUserAsync()).ReturnsAsync(user);

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            apiMock.Object,
            new Mock<ILocalStorageService>().Object);

        // Act
        var updatedUser = RandomGenerator.GenerateRandomUser();

        await service.UpdateUserAsync(updatedUser);

        // Assert
        apiMock.Verify(m => m.UpdateUserAsync(user.Id, updatedUser), Times.Once);
    }    

    [Fact]
    public async Task UserService_SetUserInfoAsync_SetsUserInfo()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IUserApi>().Object,
            localStorageServiceMock.Object);

        // Act
        await service.SetUserInfoAsync(user);

        // Assert
        localStorageServiceMock.Verify(m => m.SetItemAsync(Constants.UserId, user.Id, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.Verify(m => m.SetItemAsStringAsync(Constants.UserEmail, user.Email!, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.VerifyNoOtherCalls();        
    }

    [Fact]
    public async Task UserService_SetTokenInfoAsync_SetsTokenInfo()
    {
        // Arrange
        var response = RandomGenerator.GenerateRandomTokenResponse();

        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IUserApi>().Object,
            localStorageServiceMock.Object);

        // Act
        await service.SetTokenInfoAsync(response);

        // Assert
        localStorageServiceMock.Verify(m => m.SetItemAsStringAsync(Constants.UserToken, response.Token!, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.Verify(m => m.SetItemAsync(Constants.ExpiresAt, response.Expires, It.IsAny<CancellationToken>()), Times.Once);
        localStorageServiceMock.VerifyNoOtherCalls();        
    }

    [Fact]
    public async Task UserService_Logout_LogsUserOut()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUser();

        var localStorageServiceMock = new Mock<ILocalStorageService>();

        var service = new UserService(
            new Mock<ILogger<UserService>>().Object,
            new Mock<IUserApi>().Object,
            localStorageServiceMock.Object);

        // Act
        await service.LogoutAsync();

        // Assert
        localStorageServiceMock.Verify(m => m.RemoveItemsAsync(new List<string>
            {
                Constants.UserToken,
                Constants.UserId,
                Constants.UserEmail,
                Constants.ExpiresAt
            }, 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}