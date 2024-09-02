using QrToPay.Models.Responses;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Authentication;

public partial class LoginViewModel : ViewModelBase
{
    private readonly AuthService _authService;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    private string? emailPhone;

    [ObservableProperty]
    private string? password;

    [RelayCommand]
    private async Task Login()
    {
        if(IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(EmailPhone))
            {
                ErrorMessage = "Email lub numer telefonu nie może być pusty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Hasło nie może być puste.";
                return;
            }

            var loginResult = await _authService.LoginAsync(EmailPhone, Password);

            if (loginResult.IsSuccess)
            {
                await Shell.Current.GoToAsync("///MainPage");
                EmailPhone = null;
                Password = null;
            }
            else
            {
                ErrorMessage = loginResult.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        await NavigateAsync("/CreateUserPage");
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await NavigateAsync("/ResetPasswordPage");
    }
}