using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
        
        await viewModel.RedirectIfLogedInAsync();
    }
}