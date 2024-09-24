namespace QrToPay.ViewModels.Common;

public partial class ShellViewModel : ViewModelBase
{
    [RelayCommand]
    private Task NavigateToPage(string pageName) => NavigateAsync(pageName);

    [RelayCommand]
    private async Task Logout()
    {
        SecureStorage.Remove("AuthToken");
        SecureStorage.Remove("UserUUID");

        //_qrCodeStorageeService.CleanAllQrCodeFiles(); jak tego tu użyć żeby usuwało zawsze po wyjścu? czy powinno raczej tak
        Shell.Current.FlyoutIsPresented = false;
        await NavigateAsync($"///{nameof(LoginPage)}");
    }
}