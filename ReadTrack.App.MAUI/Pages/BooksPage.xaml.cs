using Microsoft.Maui.Controls;
using ReadTrack.App.MAUI.ViewModels;

namespace ReadTrack.App.MAUI.Pages;

public partial class BooksPage : ContentPage
{
    private readonly BookViewModel viewModel;

    public BooksPage(BookViewModel viewModel)
    {
        this.viewModel = viewModel;

        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        BindingContext = viewModel;
        await viewModel.GetAsync();
    }
}