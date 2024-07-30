namespace QrToPay.ViewModels.Base;

public partial class ViewModelBase : ObservableObject
{
    private bool isNavigating;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy && !isNavigating;

    protected async Task NavigateAsync(string route)
    {
        if (isNavigating)
            return;

        isNavigating = true;
        try
        {
            await Shell.Current.GoToAsync(route);
        }
        finally
        {
            await Task.Delay(300);
            isNavigating = false;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", false);
    }
}