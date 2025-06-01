using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.Services;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI;

public partial class AppShell : Shell
{

    private readonly MenuViewModel viewModel;

    public AppShell(MenuViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();

        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(BooksPage), typeof(BooksPage));
        Routing.RegisterRoute(nameof(DashboardPage),typeof(DashboardPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = viewModel;
    }
}
