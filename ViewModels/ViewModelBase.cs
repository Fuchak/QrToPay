namespace QrToPay.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    private bool _isNavigating;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy && !_isNavigating;

    protected async Task NavigateAsync(string route)
    {
        if (_isNavigating)
            return;

        _isNavigating = true;
        try
        {
            await Shell.Current.GoToAsync(route);
        }
        finally
        {
            await Task.Delay(300);
            _isNavigating = false;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", false);
    }
}
