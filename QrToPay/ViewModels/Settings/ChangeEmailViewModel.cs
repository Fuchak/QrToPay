using System.Net.Http.Json;
using System.Diagnostics;
using QrToPay.Models.Responses;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using QrToPay.Models.Requests;
using QrToPay.Models.Enums;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Settings;
public partial class ChangeEmailViewModel: ViewModelBase
{
    private readonly UserService _userService;

    public ChangeEmailViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string? newEmail;

    [ObservableProperty]
    private string? password;

    [RelayCommand]
    private async Task RequestEmailChange()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(NewEmail) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Wszystkie pola są wymagane.";
                return;
            }

            if (!ValidationHelper.IsEmail(NewEmail))
            {
                ErrorMessage = "Podaj poprawny adres e-mail.";
                return;
            }

            ChangeRequest changeRequest = new()
            {
                NewValue = NewEmail,
                Password = Password,
                ChangeType = ChangeType.Email
            };

            var result = await _userService.RequestChangeAsync(changeRequest);

            if (result.IsSuccess && result.Data != null)
            {
                NotificationRequest notificationRequest = new()
                {
                    Title = "Kod weryfikacyjny",
                    Description = $"Twój kod weryfikacyjny to: {result.Data.VerificationCode}",
                    ReturningData = "VerificationCode",
                    NotificationId = 1337
                };
                await LocalNotificationCenter.Current.Show(notificationRequest);

                bool verificationResult = await VerificationCodeHelper.VerifyCodeAsync(result.Data.VerificationCode);

                if (verificationResult)
                {
                    VerifyChangeRequest verifyRequest = new()
                    {
                        VerificationCode = result.Data.VerificationCode,
                        ChangeType = ChangeType.Email
                    };

                    var verifyResult = await _userService.VerifyChangeAsync(verifyRequest);
                    if (verifyResult.IsSuccess)
                    {
                        await SecureStorage.SetAsync(SecureStorageConst.UserEmail, NewEmail);
                        await Shell.Current.DisplayAlert("Sukces", "Adres e-mail został zmieniony.", "OK");
                        NewEmail = string.Empty;
                        Password = string.Empty;
                        ErrorMessage = string.Empty;
                    }
                    else
                    {
                        ErrorMessage = verifyResult.ErrorMessage;
                    }
                }
                else
                {
                    ErrorMessage = "Nieprawidłowy kod weryfikacyjny.";
                }
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}