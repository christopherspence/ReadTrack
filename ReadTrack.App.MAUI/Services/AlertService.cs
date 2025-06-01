
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Application = Microsoft.Maui.Controls.Application;


namespace ReadTrack.App.MAUI.Services;

public class AlertService : BaseService<AlertService>, IAlertService
{
    public AlertService(ILogger<AlertService> logger) : base(logger)
    {
    }

    public async Task DisplayAlertAsync(string title, string message, string cancel = "OK")
    {
        // TODO: find a better way to do this
        var shell = Shell.Current;

        if (shell != null)
        {
            await shell.DisplayAlert(title, message, cancel);
        }

        var mainPage = Application.Current?.Windows?[0]?.Page;
        if (mainPage != null)
        {
            await mainPage.DisplayAlert(title, message, cancel);
            return;
        }        
    }
}
