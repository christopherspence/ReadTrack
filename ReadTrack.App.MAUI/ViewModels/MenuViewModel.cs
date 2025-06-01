using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;

namespace ReadTrack.App.MAUI.ViewModels;

public class MenuViewModel : BaseViewModel
{
    private readonly IAlertService alertService;
    private readonly IPageService pageService;
    private readonly IUserService userService;

    public MenuViewModel(IAlertService alertService, IPageService pageService, IUserService userService)
        : base()
    {
        this.alertService = alertService;
        this.pageService = pageService;
        this.userService = userService;
    }

    public ICommand? MenuItemCommand { get; private set; }

    public override void Init()
    {
        base.Init();
         MenuItemCommand = new Command(async text => await MenuItem_Clicked(text.ToString() ?? string.Empty));
    }

    private async Task MenuItem_Clicked(string text)
    {
        switch (text.ToLower())
        {
            case "dashboard":
                await pageService.GoToAsync($"/{nameof(DashboardPage)}");
                break;
            case "books":
                await pageService.GoToAsync($"/{nameof(BooksPage)}");
                break;
            case "profile":
                await alertService.DisplayAlertAsync("Nope", "Not Implemented Yet", "OK");
                break;
            case "logout":
                await userService.LogOutAsync();
                pageService.SwitchToLoginPage();
                break;
        }
    }
}