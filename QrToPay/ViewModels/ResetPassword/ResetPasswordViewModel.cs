using System.Net.Http.Json;
using Plugin.LocalNotification;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Enums;
using QrToPay.View;
using QrToPay.Services.Api;
using Org.Apache.Http.Protocol;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.ResetPassword;

public partial class ResetPasswordViewModel : ViewModelBase
{
    private readonly UserService _userService;

    public ResetPasswordViewModel(UserService userService)
    {
        _userService = userService;
    }

    public static string? SharedEmailOrPhone { get; set; }

    [ObservableProperty]
    private string? emailPhone;

    [RelayCommand]
    private async Task Confirm()
    {
        if(IsBusy) return;
        try
        {
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(EmailPhone))
            {
                ErrorMessage = "Podaj email lub numer telefonu.";
                return;
            }

            ChangeType changeType;

            if (ValidationHelper.IsEmail(EmailPhone))
            {
                changeType = ChangeType.Email;
            }
            else if (ValidationHelper.IsPhoneNumber(EmailPhone))
            {
                changeType = ChangeType.Phone;
            }
            else
            {
                ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                return;
            }

            ResetPasswordRequest request = new()
            {
                Contact = EmailPhone,
                ChangeType = changeType
            };

            var result = await _userService.CheckAccountAsync(request);
            if (result.IsSuccess)
            {
                var changeResponse = result.Data;

                if (changeResponse != null)
                {
                    //Cant set notification time in release mode
                    //https://github.com/thudugala/Plugin.LocalNotification/issues/516
                    NotificationRequest notificationRequest = new()
                    {
                        Title = "Kod weryfikacyjny",
                        Description = $"Twój kod weryfikacyjny to: {changeResponse.VerificationCode}",
                        ReturningData = "VerificationCode",
                        NotificationId = 1337
                        /*Schedule =
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5) // Można dostosować czas powiadomienia
                        }*/
                    };
                    await LocalNotificationCenter.Current.Show(notificationRequest);

                    SharedEmailOrPhone = EmailPhone;
                    await NavigateAsync(nameof(ResetPasswordConfirmPage));
                }
                else
                {
                    ErrorMessage = "Błąd podczas odczytywania kodu weryfikacyjnego.";
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