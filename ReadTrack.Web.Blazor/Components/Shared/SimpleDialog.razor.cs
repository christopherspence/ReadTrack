using Microsoft.AspNetCore.Components;

namespace ReadTrack.Web.Blazor.Components.Shared;

public partial class SimpleDialog : ComponentBase
{
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public string? Message { get; set; }
    [Parameter]
    public bool IsVisible { get; set; }
    [Parameter]
    public EventCallback OnClose { get; set; } 

    public void Show(string title, string message)
    {
        Title = title;
        Message = message;
        IsVisible = true;
        StateHasChanged();
    }

    private async Task CloseDialog()
    {
        IsVisible = false;
        await OnClose.InvokeAsync();
        StateHasChanged();
    }
}