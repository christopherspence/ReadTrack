using Microsoft.Maui.Controls;

namespace ReadTrack.App.MAUI.Views;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();
    }

    public string ViewTitle
    {
        get => (string)GetValue(ViewTitleProperty);
        set => SetValue(ViewTitleProperty, value);
    }

    public static readonly BindableProperty ViewTitleProperty = BindableProperty.Create(nameof(ViewTitle), typeof(string), typeof(HeaderView), string.Empty);

    public string ViewDescription
    {
        get => (string)GetValue(ViewDescriptionProperty);
        set => SetValue(ViewDescriptionProperty, value);
    }

     public static readonly BindableProperty ViewDescriptionProperty = BindableProperty.Create(nameof(ViewDescription), typeof(string), typeof(HeaderView), string.Empty);
}