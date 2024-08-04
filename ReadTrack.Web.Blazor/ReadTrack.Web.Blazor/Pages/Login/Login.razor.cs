using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace ReadTrack.Web.Blazor.Pages.Login;

public partial class LoginComponent
{
    private EditForm form;
    private bool loading = false;

    private string email;
    private string password;

    private bool HasError(string controlName, string errorName)
    {
        // Implement error checking logic here
        return false; // Placeholder
    }

    private async Task Login()
    {
        loading = true;

        try
        {
            await AuthService.Login(email, password);

            NavigationManager.NavigateTo("");
        }
        catch (Exception e)
        {
            var parameters = new DialogParameters
            {
                { "Title", "Error" },
                { "Message", "An error occurred while logging in" }
            };
            DialogService.Show<SimpleDialogComponent>("Error", parameters);
        }

        loading = false;
    }
}

