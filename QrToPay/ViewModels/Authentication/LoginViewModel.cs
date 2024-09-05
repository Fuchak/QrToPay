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
        try
        {
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
                await NavigateAsync($"///{nameof(MainPage)}");
                EmailPhone = string.Empty; ;
                Password = string.Empty; ;
            }
            else
            {
                ErrorMessage = loginResult.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        await NavigateAsync(nameof(CreateUserPage));
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        await NavigateAsync(nameof(ResetPasswordPage));
    }
}