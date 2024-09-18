namespace QrToPay.ViewModels.Base;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    string? errorMessage;

    [ObservableProperty]
    private bool isPassword = true;

    [ObservableProperty]
    private string passwordVisibilityIcon = "\ue8f4";

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy;

    protected async Task NavigateAsync(string route)
    {
        if (IsBusy)
            return;

        IsBusy = true;
        try
        {
            await Shell.Current.GoToAsync(route, false);
        }
        finally
        {
            ErrorMessage = null;
            await Task.Delay(100);
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPassword = !IsPassword;

        PasswordVisibilityIcon = IsPassword 
            ? "\ue8f4" 
            : "\ue8f5";
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..", false);
    }
}