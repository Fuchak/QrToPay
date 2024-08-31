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

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Login()
    {
        if(IsBusy) return;
        try
        {
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

            ApiResponse loginResult = await _authService.LoginAsync(EmailPhone, Password);

            if (loginResult.Success)
            {
                EmailPhone = null;
                Password = null;
                ErrorMessage = null;
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                ErrorMessage = loginResult.Message;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        ErrorMessage = null; // TODO To czyści kod błędu żeby po powrocie na stronę go nie było ale potrzebujemy go jakoś globalniej bo tak co chwile go pisać to meh
        //await Shell.Current.GoToAsync("/CreateUserPage");
        await NavigateAsync("/CreateUserPage");
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        ErrorMessage = null; // To samo można dodać do ekranów Rejestracji i Resetowania hasła ale to potem jak nie będzie lepszej opcji globalnej
        //await Shell.Current.GoToAsync("/ResetPasswordPage");
        await NavigateAsync("/ResetPasswordPage");
    }
}