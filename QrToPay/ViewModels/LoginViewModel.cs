﻿namespace QrToPay.ViewModels;

public partial class LoginViewModel(AuthService authService) : ViewModelBase
{
    [ObservableProperty]
    private string? emailPhone;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Login()
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

        var loginResult = await authService.Login(EmailPhone, Password);

        if (loginResult.Success)
        {
            // Przejdź do strony głównej po udanym zalogowaniu
            await Shell.Current.GoToAsync("///MainPage");
        }
        else
        {
            ErrorMessage = loginResult.ErrorMessage;
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        ErrorMessage = null; // To czyści kod błędu żeby po powrocie na stronę go nie było ale potrzebujemy go jakoś globalniej bo tak co chwile go pisać to meh
        await Shell.Current.GoToAsync("/CreateUserPage");
    }

    [RelayCommand]
    private async Task ForgotPassword()
    {
        ErrorMessage = null; // To samo można dodać do ekranów Rejestracji i Resetowania hasła ale to potem jak nie będzie lepszej opcji globalnej
        await Shell.Current.GoToAsync("/ResetPasswordPage");
    }
}