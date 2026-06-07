using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReadTrack.API.Controllers;
using ReadTrack.API.Services;
using ReadTrack.API.Tests.Utilities;
using Xunit;

namespace ReadTrack.API.Tests.Controllers;

public class AnalyticsControllerTests : BaseControllerTests
{
    [Fact]
    public async Task CanGetBooksRead()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var now = DateTime.UtcNow;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var expected = RandomGenerator.GenerateRandomIntAnalytics();
            
        var analyticsServiceMock = new Mock<IAnalyticsService>();
        analyticsServiceMock.Setup(m => m.GetBooksReadAsync(user.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expected);
        
        var controller = new AnalyticsController(new Mock<ILogger<AnalyticsController>>().Object, analyticsServiceMock.Object, userServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email ?? string.Empty)
        };

        // Act
        var start = new DateTime(now.Year, now.Month, 1);
        var end = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        var response = await controller.GetBooksReadAsync(start, end);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        analyticsServiceMock.Verify(m => m.GetBooksReadAsync(user.Id, start, end), Times.Once);   
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email ?? string.Empty), Times.Once);
    }

    [Fact]
    public async Task CanGetPagesRead()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var now = DateTime.UtcNow;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var expected = RandomGenerator.GenerateRandomIntAnalytics();
            
        var analyticsServiceMock = new Mock<IAnalyticsService>();
        analyticsServiceMock.Setup(m => m.GetBooksReadAsync(user.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expected);
        
        var controller = new AnalyticsController(new Mock<ILogger<AnalyticsController>>().Object, analyticsServiceMock.Object, userServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email ?? string.Empty)
        };

        // Act
        var start = new DateTime(now.Year, now.Month, 1);
        var end = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        var response = await controller.GetPagesReadAsync(start, end);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        analyticsServiceMock.Verify(m => m.GetPagesReadAsync(user.Id, start, end), Times.Once);   
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email ?? string.Empty), Times.Once);
    }

    [Fact]
    public async Task CanGetReadingTime()
    {
        // Arrange
        var user = RandomGenerator.GenerateRandomUserModel();

        var now = DateTime.UtcNow;

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var expected = RandomGenerator.GenerateRandomIntAnalytics();
            
        var analyticsServiceMock = new Mock<IAnalyticsService>();
        analyticsServiceMock.Setup(m => m.GetReadingTimeAsync(user.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expected);
        
        var controller = new AnalyticsController(new Mock<ILogger<AnalyticsController>>().Object, analyticsServiceMock.Object, userServiceMock.Object)
        {
            ControllerContext = CreateControllerContext(user.Email ?? string.Empty)
        };

        // Act
        var start = new DateTime(now.Year, now.Month, 1);
        var end = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        var response = await controller.GetReadingTimeAsync(start, end);

        // Assert
        response.Should().BeOfType<OkObjectResult>();

        analyticsServiceMock.Verify(m => m.GetReadingTimeAsync(user.Id, start, end), Times.Once);   
        userServiceMock.Verify(m => m.GetUserByEmailAsync(user.Email ?? string.Empty), Times.Once);
    }
}