namespace QrToPay.ViewModels.Common;

public partial class ShellViewModel : ViewModelBase
{
    [RelayCommand]
    private Task NavigateToPage(string pageName) => NavigateAsync(pageName);

    [RelayCommand]
    private async Task Logout()
    {
        if (Preferences.ContainsKey("AuthState"))
        {
            Preferences.Remove("AuthState");
        }
        Shell.Current.FlyoutIsPresented = false;
        await Shell.Current.GoToAsync("///LoginPage");  //TODO może użyjmy viewmodelbase też?
    }
}