using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace ReadTrack.Web.Blazor.Pages.Profile;

public partial class ProfileComponent : ComponentBase
{
    private EditContext form;
    private bool loading = false;
    private string imageSource = string.Empty;
    private bool imageChanged = false;
    private bool passwordChanged = false;

    private User user;

    [Inject] private DomSanitizer sanitizer { get; set; }
    [Inject] private UserService userService { get; set; }
    [Inject] private IDialogService dialog { get; set; }
    [Inject] private ISnackBarService snackBar { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetUserInfo();
        SetupForm();
    }

    private async Task GetUserInfo()
    {
        user = await userService.GetUserAsync();

        if (user != null)
        {
            form.SetFieldValue("firstName", user.FirstName);
            form.SetFieldValue("lastName", user.LastName);
            form.SetFieldValue("email", user.Email);

            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                imageSource = sanitizer.BypassSecurityTrustResourceUrl($"data:image/jpg;base64,{user.ProfilePicture}").ToString();
            }
            else
            {
                imageSource = "./assets/img/profile.png";
            }
        }
    }

    private void SetupForm()
    {
        var model = new
        {
            firstName = string.Empty,
            lastName = string.Empty,
            email = string.Empty,
            password = string.Empty,
            confirmPassword = string.Empty
        };

        form = new EditContext(model);
    }

    public bool HasError(string controlName, string errorName)
    {
        return form.GetValidationMessages(controlName).Any();
    }

    private async Task OnFileChange(InputFileChangeEventArgs e)
    {
        var file = e.GetMultipleFiles(1).FirstOrDefault();
        if (file != null)
        {
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);
            imageSource = Convert.ToBase64String(buffer);
            imageChanged = true;
        }
    }

    public async Task Submit()
    {
        if (user != null)
        {
            loading = true;

            var formValues = form.Model;

            user.FirstName = formValues.firstName;
            user.LastName = formValues.lastName;
            // TODO: disabling this for now until I can figure out the token issue after changing email
            // user.Email = formValues.email;

            if (imageChanged)
            {
                user.ProfilePicture = StringUtilities.RemoveBase64Prefix(imageSource);
            }
            if (passwordChanged)
            {
                user.Password = formValues.password;
            }

            try
            {
                await userService.UpdateUserAsync(user);

                snackBar.Show("Profile updated successfully", new SnackBarOptions
                {
                    Duration = 2000,
                    VerticalPosition = VerticalPosition.Top,
                    HorizontalPosition = HorizontalPosition.Right,
                });
            }
            catch (Exception e)
            {
                await dialog.Show<SimpleDialogComponent>("Error", new DialogParameters
                {
                    { "Title", "Error" },
                    { "Message", e.Message }
                });
            }

            loading = false;
        }
    }
}

