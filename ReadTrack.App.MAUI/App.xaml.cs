using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI;

public partial class App : Application
{
	private readonly LoginViewModel viewModel;

	public App(LoginViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		//return new Window(new AppShell());

		return new Window(new NavigationPage(new LoginPage(viewModel)));
	}
}