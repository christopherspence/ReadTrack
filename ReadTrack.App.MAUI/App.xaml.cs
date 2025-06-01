using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.Pages;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI;

public partial class App : Application
{
	public App(IServiceProvider provider)
	{
		InitializeComponent();
		Services = provider;
	}

	public IServiceProvider Services { get; private set; }


	protected override Window CreateWindow(IActivationState? activationState)
	{		
		var viewModel = Services.GetRequiredService<LoginViewModel>();
		return new Window(new NavigationPage(new LoginPage(viewModel)));
	}
}