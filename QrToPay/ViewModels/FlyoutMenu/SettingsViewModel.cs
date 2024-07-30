namespace QrToPay.ViewModels.FlyoutMenu;
public partial class SettingsViewModel : ViewModelBase
{
    [RelayCommand]
    private Task NavigateToChangePassword() => NavigateAsync(nameof(ChangePasswordPage));

    [RelayCommand]
    private Task NavigateToChangePhone() => NavigateAsync(nameof(ChangePhonePage));

    [RelayCommand]
    private Task NavigateToChangeEmail() => NavigateAsync(nameof(ChangeEmailPage));
}