using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Messages;

namespace QrToPay.ViewModels.Common;

public partial class ShellViewModel : ViewModelBase
{
    public ShellViewModel()
    {
        WeakReferenceMessenger.Default.Register<UserLogoutRequestMessage>(this, async (r, message) =>
        {
            await Logout();
        });
    }

    [RelayCommand]
    private Task NavigateToPage(string pageName) => NavigateAsync(pageName);

    [RelayCommand]
    private async Task Logout()
    {
        SecureStorage.Default.RemoveAll();

        WeakReferenceMessenger.Default.Send(new UserLogoutMessage());

        Shell.Current.FlyoutIsPresented = false;
        await NavigateAsync($"///{nameof(LoginPage)}");
    }
}