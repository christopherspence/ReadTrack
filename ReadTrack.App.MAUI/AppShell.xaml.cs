using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Pages;

namespace ReadTrack.App.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(
            nameof(DashboardPage),
            typeof(DashboardPage)
        );
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
