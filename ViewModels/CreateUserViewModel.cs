namespace QrToPay.ViewModels;

public partial class CreateUserViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [ObservableProperty]
    private string? emailPhone;

    [RelayCommand]
    private async Task Confirm()
    {
        Password = string.Empty;
        PasswordConfirm = string.Empty;
        EmailPhone = string.Empty;
        await Shell.Current.GoToAsync("CreateUserConfirmPage");
    }
}
