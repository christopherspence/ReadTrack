namespace ReadTrack.Web.Blazor.Components.Layout;

public partial class MainLayout
{
    private bool Opened { get; set; }

    void ButtonClicked() => Opened = !Opened;
}