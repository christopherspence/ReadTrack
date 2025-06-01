using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace ReadTrack.App.MAUI.Pages;

public partial class DashboardPage : ContentPage
{
	int count = 0;

	public DashboardPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

