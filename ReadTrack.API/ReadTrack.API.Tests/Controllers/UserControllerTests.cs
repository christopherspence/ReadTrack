using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Models;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Controllers;

public class UserControllerTests : BaseControllerTests
{
    [Fact]
    public async Task CanGetCurrentUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.GetCurrentUserAsync();

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);
    }

    [Fact]
    public async Task CanRegisterUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userServiceMock.Object);

        // Act
        var request = RandomGenerator.GenerateRandomCreateUserRequest();
        var response = await controller.RegisterAsync(request);

        // Assert
        userServiceMock.Verify(m => m.CreateUserAsync(request), Times.Once);
        
    }

    [Fact]
    public async Task CanUpdateUser()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<int>(), It.IsAny<User>())).ReturnsAsync(true);

        var controller = new UserController(new Mock<ILogger<UserController>>().Object, userServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email)
        };

        // Act
        var response = await controller.UpdateUserAsync(user.Id, user);

        // Assert
        response.Should().BeOfType<NoContentResult>();

        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email), Times.Once);
        userServiceMock.Verify(m => m.UpdateUserAsync(user.Id, user), Times.Once);   
    }
}