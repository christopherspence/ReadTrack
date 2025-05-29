using System.Windows.Input;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;
using Refit;

namespace ReadTrack.App.MAUI.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IUserService userService;
    private readonly IAuthService authService;

    private string? email;
    private string? password;


    public LoginViewModel(IAuthService authService, IUserService userService)
    {
        this.authService = authService;
        this.userService = userService;
    }

    public string? Email
    {
        get => email;
        set
        {
            email = value;
            RaisePropertyChanged();
        }
    }

    public string? Password
    {
        get => password;
        set
        {
            password = value;
            RaisePropertyChanged();
        }
    }

    public ICommand? LoginCommand { get; private set; }

    public override void Init()
    {
        base.Init();

        LoginCommand = new Command(async () => await LoginAsync());
    }

    public async Task RedirectIfLogedInAsync()
    {
        if (await userService.IsLoggedInAsync())
        {
            // TODO: figure out if this is right
            var appCurrent = Application.Current;
            if (appCurrent != null && appCurrent.Windows.Any())
            {
                appCurrent.Windows[0].Page = new AppShell();
            }
        }
    }

    public async Task LoginAsync()
    {
        var errorMessage = "";

        if (string.IsNullOrEmpty(Email))
        {
            errorMessage = "Please enter a user name.\n";
        }

        if (string.IsNullOrEmpty(Password))
        {
            errorMessage += "Please enter a password.";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            await DisplayAlertAsync("Validation", errorMessage);
            return;
        }

        try
        {
            await authService.LoginAsync(new()
            {
                Email = Email,
                Password = Password
            });
        }
        catch (ApiException e)
        {
            await DisplayAlertAsync("Validation", e.Message);
            return;
        }

        // token is set
        await RedirectIfLogedInAsync();
    }

    private async Task DisplayAlertAsync(string title, string message)
    {
        // TODO: find a better way to do this
        if (App.Current?.MainPage != null)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "OK");
        }

    }
}