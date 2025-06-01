namespace ReadTrack.App.MAUI.Services;

public interface IAlertService : IService
{
    Task DisplayAlertAsync(string title, string message, string ok = "OK");
}