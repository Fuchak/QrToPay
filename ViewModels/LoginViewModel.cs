namespace QrToPay.ViewModels;

public partial class LoginViewModel(AuthService authService) : ViewModelBase
{
    private readonly AuthService _authService = authService;

    [ObservableProperty]
    private string? emailPhone;

    [ObservableProperty]
    private string? password;

    [RelayCommand]
    private async Task Login()
    {
        EmailPhone = string.Empty;
        Password = string.Empty; 

        _authService.Login();
        await Shell.Current.GoToAsync("///MainPage");
    }

    [RelayCommand]
    private async Task Register()
    {
        await Shell.Current.GoToAsync("/CreateUserPage");
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await Shell.Current.GoToAsync("/ResetPasswordPage");
    }
}