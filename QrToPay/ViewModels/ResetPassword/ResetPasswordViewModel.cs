﻿using System.Net.Http.Json;
using Plugin.LocalNotification;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Enums;
using QrToPay.View;

namespace QrToPay.ViewModels.ResetPassword;
//TODO to z tym kodem widziałem gdzieś już
public partial class ResetPasswordViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ResetPasswordViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public static string? SharedEmailOrPhone { get; set; }

    [ObservableProperty]
    private string? emailPhone;

    [RelayCommand]
    private async Task Confirm()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EmailPhone))
            {
                ErrorMessage = "Podaj email lub numer telefonu.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

            ChangeType changeType;
            object requestData;

            if (ValidationHelper.IsEmail(EmailPhone))
            {
                changeType = ChangeType.Email;
                requestData = new { Contact = EmailPhone, ChangeType = changeType };
            }
            else if (ValidationHelper.IsPhoneNumber(EmailPhone))
            {
                changeType = ChangeType.Phone;
                requestData = new { Contact = EmailPhone, ChangeType = changeType };
            }
            else
            {
                ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                IsBusy = false;
                return;
            }

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Auth/checkAccount", requestData);
            if (response.IsSuccessStatusCode)
            {
                ChangeResponse? changeResponse = await response.Content.ReadFromJsonAsync<ChangeResponse>();

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
                ErrorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response) 
                    ?? "Błąd podczas resetowania hasła. Spróbuj ponownie.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
    }
}