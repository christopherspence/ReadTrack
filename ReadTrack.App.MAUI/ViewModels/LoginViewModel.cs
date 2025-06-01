using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Services;
using Refit;

namespace ReadTrack.App.MAUI.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAlertService alertService;
    private readonly IAuthService authService;
    private readonly IUserService userService;
    

    private string? email;
    private string? password;


    public LoginViewModel(IAlertService alertService, IAuthService authService, IUserService userService)
    {
        this.alertService = alertService;
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
            await alertService.DisplayAlertAsync("Validation", errorMessage);
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
            await alertService.DisplayAlertAsync("Validation", e.Message);
            return;
        }

        // token is set
        await RedirectIfLogedInAsync();
    }    
}