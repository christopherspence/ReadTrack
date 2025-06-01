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

        var viewModel = new LoginViewModel(alertServiceMock.Object, authServiceMock.Object, new Mock<IUserService>().Object);

        // Act
        await viewModel.LoginAsync();

        // Assert
        alertServiceMock.Verify(m => m.DisplayAlertAsync("Validation", "Please enter a user name.\nPlease enter a password.", "OK"), Times.Once);
        alertServiceMock.VerifyNoOtherCalls();

        authServiceMock.Verify(m => m.LoginAsync(It.IsAny<AuthRequest>()), Times.Never);
    }

    [Fact]
    public async Task LoginViewModel_LoginAsync_LogsInWithValidInfo()
    {
        // Arrange 
        var alertServiceMock = new Mock<IAlertService>();
        var authServiceMock = new Mock<IAuthService>();

        var email = RandomGenerator.CreateEmailAddress();
        var password = RandomGenerator.CreatePlaceName();
        var viewModel = new LoginViewModel(alertServiceMock.Object, authServiceMock.Object, new Mock<IUserService>().Object)
        {
            // Act
            Email = email,
            Password = password
        };

        await viewModel.LoginAsync();

        // Assert
        alertServiceMock.Verify(m => m.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        authServiceMock.Verify(m => m.LoginAsync(It.Is<AuthRequest>(r => r.Email == email && r.Password == password)), Times.Once);
        authServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginViewModel_RedirectIfLoggedIn_WillRedirectIfTokenExists()
    {
        // Arrange 
        
        var userServiceMock = new Mock<IUserService>();
        userServiceMock.Setup(m => m.IsLoggedInAsync()).ReturnsAsync(true);

        var viewModel = new LoginViewModel(new Mock<IAlertService>().Object, new Mock<IAuthService>().Object, userServiceMock.Object);

        // Act
        await viewModel.RedirectIfLogedInAsync();

        // Assert
        // TODO: figure how to test if app gets set to AppShell

        userServiceMock.Verify(m => m.IsLoggedInAsync(), Times.Once);
        userServiceMock.VerifyNoOtherCalls();
    }
}