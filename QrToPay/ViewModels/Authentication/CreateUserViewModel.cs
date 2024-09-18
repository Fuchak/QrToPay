using System.Net.Http.Json;
using Plugin.LocalNotification;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Authentication;

public partial class CreateUserViewModel : ViewModelBase
{
    private readonly UserService _userService;

    public CreateUserViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [ObservableProperty]
    private string? emailPhone;

    [RelayCommand]
    private async Task Confirm()
    {
        if(IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(EmailPhone) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
            {
                ErrorMessage = "Wszystkie pola są wymagane.";
                return;
            }

            if (Password != PasswordConfirm)
            {
                ErrorMessage = "Hasła nie są zgodne.";
                return;
            }

            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper) || !Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ErrorMessage = "Hasło nie spełnia wymagań.";
                return;
            }

            if (!ValidationHelper.IsEmail(EmailPhone) && !ValidationHelper.IsPhoneNumber(EmailPhone))
            {
                ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                return;
            }

            CreateUserRequest request = ValidationHelper.IsEmail(EmailPhone)
                ? new CreateUserRequest { Email = EmailPhone, PasswordHash = Password }
                : new CreateUserRequest { PhoneNumber = EmailPhone, PasswordHash = Password };

            var registerResult = await _userService.RegisterUserAsync(request);

            if (!registerResult.IsSuccess)
            {
                ErrorMessage = registerResult.ErrorMessage 
                    ?? "Rejestracja nie powiodła się. Spróbuj ponownie.";
                return;
            }

            var registerResponse = registerResult.Data;

            NotificationRequest notification = new()
            {
                Title = "Kod weryfikacyjny",
                Description = $"Twój kod weryfikacyjny to: {registerResponse!.VerificationCode}",
                ReturningData = "VerificationCode",
                NotificationId = 1337
            };

            await LocalNotificationCenter.Current.Show(notification);

            bool verificationResult = await VerificationCodeHelper.VerifyCodeAsync(registerResponse.VerificationCode);

            if (!verificationResult)
            {
                ErrorMessage = "Nieprawidłowy kod weryfikacyjny.";
                return;
            }

            VerifyCreateUserRequest verifyRequest = new()
            {
                EmailOrPhone = registerResponse.EmailOrPhone,
                VerificationCode = registerResponse.VerificationCode
            };

            var verifyResult = await _userService.VerifyUserAsync(verifyRequest);

            if (verifyResult.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Sukces", "Kod weryfikacyjny jest poprawny.", "OK");
                await NavigateAsync($"//{nameof(LoginPage)}");
                Password = string.Empty;
                PasswordConfirm = string.Empty;
            }
            else
            {
                ErrorMessage = verifyResult.ErrorMessage
                    ?? "Błąd podczas weryfikacji użytkownika. Spróbuj ponownie.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}