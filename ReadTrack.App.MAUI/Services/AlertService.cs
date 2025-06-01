
using Microsoft.Extensions.Logging;

namespace ReadTrack.App.MAUI.Services;

public class AlertService : BaseService<AlertService>, IAlertService
{
    public AlertService(ILogger<AlertService> logger) : base(logger)
    {
    }

    public async Task DisplayAlertAsync(string title, string message, string ok = "OK")
    {
        // TODO: find a better way to do this
        var shell = Shell.Current;

        if (shell != null)
        {
            await shell.DisplayAlert(title, message, ok);
        }

        var mainPage = Application.Current?.Windows?[0]?.Page;
        if (mainPage != null)
        {
            await mainPage.DisplayAlert(title, message, "OK");
            return;
        }        
    }
}
