namespace QrToPay.ViewModels;

public partial class ResetPasswordConfirmViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [RelayCommand]
    private async Task Confirm()
    {
        Code = string.Empty;
        Password = string.Empty;
        PasswordConfirm = string.Empty;

        await Shell.Current.GoToAsync("///LoginPage");
    }
}