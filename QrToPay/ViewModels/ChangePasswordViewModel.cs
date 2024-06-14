namespace QrToPay.ViewModels;
public partial class ChangePasswordViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? oldpassword;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [RelayCommand]
    private async Task Confirm()
    {
        Oldpassword = string.Empty;
        Password = string.Empty;
        PasswordConfirm = string.Empty;

        await Shell.Current.DisplayAlert("Potwierdzenie", "Hasło zostało zmienione", "OK");
    }
}