using Moq;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.ViewModels;
using Xunit;

namespace ReadTrack.App.MAUI.Tests.ViewModels;

public class MenuViewModelTests
{
    [Theory]
    [InlineData("dashboard")]
    [InlineData("books")]
    [InlineData("profile")]
    [InlineData("logout")]
    public void MenuViewModel_ExecuteMenuItemCommand_ExecutesProperPath(string text)
    {
        // Arrange
        var alertServiceMock = new Mock<IAlertService>();
        var pageServiceMock = new Mock<IPageService>();
        var userServiceMock = new Mock<IUserService>();

        var viewModel = new MenuViewModel(alertServiceMock.Object, pageServiceMock.Object, userServiceMock.Object);

        // Act
        viewModel.MenuItemCommand!.Execute(text);

        // Assert
        switch (text)
        {
            case "dashboard":
                pageServiceMock.Verify(m => m.GoToAsync($"/{nameof(DashboardPage)}"), Times.Once);
                pageServiceMock.VerifyNoOtherCalls();
                break;
            case "books":
                pageServiceMock.Verify(m => m.GoToAsync($"/{nameof(BooksPage)}"), Times.Once);
                pageServiceMock.VerifyNoOtherCalls();
                break;
            case "profile":
                alertServiceMock.Verify(m => m.DisplayAlertAsync("Nope", "Not Implemented Yet", "OK"), Times.Once);
                alertServiceMock.VerifyNoOtherCalls();
                break;
            case "logout":
                pageServiceMock.Verify(m => m.SwitchToLoginPage(), Times.Once);
                pageServiceMock.VerifyNoOtherCalls();

                userServiceMock.Verify(m => m.LogOutAsync(), Times.Once);
                userServiceMock.VerifyNoOtherCalls();
                break;     
        }
    }
}