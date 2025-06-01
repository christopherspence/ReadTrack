using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Moq;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.Tests.Utilities;
using ReadTrack.App.MAUI.ViewModels;
using ReadTrack.Shared.Models.Requests;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.ViewModels;

public class LoginViewModelTests
{
    [Fact]
    public async Task LoginViewModel_LoginAsync_ShowsAlertWithMissingInfo()
    {
        // Arrange 
        var alertServiceMock = new Mock<IAlertService>();
        var authServiceMock = new Mock<IAuthService>();

        var viewModel = new LoginViewModel(
            alertServiceMock.Object,
            authServiceMock.Object,
            new Mock<IPageService>().Object,
            new Mock<IUserService>().Object);

        // Act
        await viewModel.LoginAsync();

        // Assert
        alertServiceMock.Verify(m => m.DisplayAlertAsync("Validation", "Please enter a user name.\nPlease enter a password.", "OK"), Times.Once);
        alertServiceMock.VerifyNoOtherCalls();

        authServiceMock.Verify(m => m.LoginAsync(It.IsAny<AuthRequest>()), Times.Never);
    }

    [Fact]
    public void LoginViewModel_ExecuteLoginCommand_LogsInWithValidInfo()
    {
        // Arrange 
        var alertServiceMock = new Mock<IAlertService>();
        var authServiceMock = new Mock<IAuthService>();
        var pageServiceMock = new Mock<IPageService>();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.IsLoggedInAsync()).ReturnsAsync(true);

        var email = RandomGenerator.CreateEmailAddress();
        var password = RandomGenerator.CreatePlaceName();
        var viewModel = new LoginViewModel(
            alertServiceMock.Object,
            authServiceMock.Object,
            pageServiceMock.Object,
            userServiceMock.Object)
        {
            // Act
            Email = email,
            Password = password
        };

        viewModel.LoginCommand?.Execute(null);

        // Assert
        alertServiceMock.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        authServiceMock.Verify(m => m.LoginAsync(It.Is<AuthRequest>(r => r.Email == email && r.Password == password)), Times.Once);
        authServiceMock.VerifyNoOtherCalls();

        pageServiceMock.Verify(m => m.SwitchToDashboard(), Times.Once);
        pageServiceMock.VerifyNoOtherCalls();

        userServiceMock.Verify(m => m.IsLoggedInAsync(), Times.Once);
        userServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginViewModel_RedirectIfLoggedIn_WillRedirectIfTokenExists()
    {
        // Arrange 
        var pageServiceMock = new Mock<IPageService>();

        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.IsLoggedInAsync()).ReturnsAsync(true);

        var viewModel = new LoginViewModel(
            new Mock<IAlertService>().Object,
            new Mock<IAuthService>().Object,
            pageServiceMock.Object,
            userServiceMock.Object);

        // Act
        await viewModel.RedirectIfLogedInAsync();

        // Assert
        pageServiceMock.Verify(m => m.SwitchToDashboard(), Times.Once);
        pageServiceMock.VerifyNoOtherCalls();

        userServiceMock.Verify(m => m.IsLoggedInAsync(), Times.Once);
        userServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void LoginViewModel_ClearValues_ClearsValues()
    {
        // Arrange 
        var viewModel = new LoginViewModel(
            new Mock<IAlertService>().Object,
            new Mock<IAuthService>().Object,
            new Mock<IPageService>().Object,
            new Mock<IUserService>().Object);

        // Act
        viewModel.Email = RandomGenerator.CreateEmailAddress();
        viewModel.Password = RandomGenerator.CreatePlaceName();

        viewModel.ClearValues();

        // Assert
        viewModel.Email.Should().BeEmpty();
        viewModel.Password.Should().BeEmpty();
    }
}