using Microsoft.AspNetCore.Components;
using ReadTrack.Web.Blazor.Models;
using ReadTrack.Web.Blazor.Services;

namespace ReadTrack.Web.Blazor.Components.Pages.Login;

public partial class Login : ComponentBase
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    [Inject]
    public IAuthService? AuthService { get; set; }
    
    private readonly LoginModel loginModel = new();
    private bool loading = false;
    private string errorMessage = string.Empty;

    private async Task HandleLoginAsync()
    {
        loading = true;
        // Clear previous messages
        errorMessage = string.Empty;

        try
        {
            if (AuthService != null)
            {
                var success = await AuthService.LoginAsync(loginModel?.Email ?? string.Empty, loginModel?.Password ?? string.Empty);

                if (success)
                {
                    NavigationManager?.NavigateTo("/dashboard");
                }
                else
                {
                    errorMessage = "Login failed. Please check your credentials.";
                }
            }

        }
        catch (Exception ex)
        {
            errorMessage = "An error occurred while logging in. Please try again.";

            await ShowErrorDialogAsync(errorMessage, ex.Message);
        }
        finally
        {
            loading = false;
        }
    }

    // TODO: show dialog
    private async Task ShowErrorDialogAsync(string title, string message)
    {
        Console.WriteLine($"Login failed: {title} {message}");         
    }
}