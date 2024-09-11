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

            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                return;
            }

            ChangeRequest changeRequest = new()
            {
                UserId = userId,
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
                        UserId = userId,
                        VerificationCode = result.Data.VerificationCode,
                        ChangeType = ChangeType.Email
                    };

                    var verifyResult = await _userService.VerifyChangeAsync(verifyRequest);
                    if (verifyResult.IsSuccess)
                    {
                        Preferences.Set("UserEmail", NewEmail);
                        await Shell.Current.DisplayAlert("Sukces", "Adres e-mail został zmieniony.", "OK");
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
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
            //TODO : Czy to musi czyścić tu czy w OnAppearing? chyba w OnAppearing żeby użytkownik po złym wpisaniu nie musiał wpisywać od nowa
            //NewEmail = string.Empty;
            //Password = string.Empty;
            //ErrorMessage = string.Empty;
        }
    }
}