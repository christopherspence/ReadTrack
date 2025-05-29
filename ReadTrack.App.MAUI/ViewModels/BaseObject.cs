using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReadTrack.App.MAUI.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected BaseViewModel()
    {
        Init();
    }

    public virtual void Init() { }

    public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
