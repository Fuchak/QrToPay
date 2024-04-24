namespace QrToPay.ViewModels;

public partial class ShellViewModel : ViewModelBase
{
    [RelayCommand]
    private async Task NavigateToPage(string route)
    {
        Shell.Current.FlyoutIsPresented = false;
        await Shell.Current.GoToAsync(route);
    }

    [RelayCommand]
    private async Task Logout()
    {
        if (Preferences.ContainsKey("AuthState"))
        {
            Preferences.Remove("AuthState");
        }
        Shell.Current.FlyoutIsPresented = false;
        await Shell.Current.GoToAsync("///LoginPage");
    }

    [RelayCommand]
    private void CloseFlyout()
    {
        Shell.Current.FlyoutIsPresented = false;
    }
}
