using System.Net.Http.Json;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using QrToPay.Models.Responses;
using QrToPay.Models.Enums;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Settings;

public partial class ChangePhoneViewModel : ViewModelBase
{
    private readonly UserService _userService;

    public ChangePhoneViewModel(UserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string? newPhoneNumber;

    [ObservableProperty]
    private string? password;

    [RelayCommand]
    private async Task RequestPhoneChange()
    {
        if (IsBusy) return;        
        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(NewPhoneNumber) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Wszystkie pola są wymagane.";
                return;
            }

            if (!ValidationHelper.IsPhoneNumber(NewPhoneNumber))
            {
                ErrorMessage = "Podaj poprawny numer telefonu (+48 i 9 cyfr).";
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
                NewValue = NewPhoneNumber,
                Password = Password,
                ChangeType = ChangeType.Phone
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
                        ChangeType = ChangeType.Phone
                    };

                    var verifyResult = await _userService.VerifyChangeAsync(verifyRequest);
                    if (verifyResult.IsSuccess)
                    {
                        Preferences.Set("UserPhone", NewPhoneNumber);
                        await Shell.Current.DisplayAlert("Sukces", "Numer telefonu został zmieniony.", "OK");
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
            //NewPhoneNumber = string.Empty;
            //Password = string.Empty;
            //ErrorMessage = string.Empty;
        }
    }
}