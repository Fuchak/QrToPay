﻿using System.Net.Http.Json;
using System.Diagnostics;
using QrToPay.Models.Responses;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using Newtonsoft.Json;
using QrToPay.Models.Requests;
using QrToPay.Models.Enums;

namespace QrToPay.ViewModels.Settings;
public partial class ChangeEmailViewModel: ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChangeEmailViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    private string? newEmail;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task RequestEmailChange()
    {
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

        IsBusy = true;
        try
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                return;
            }

            Debug.WriteLine($"UserId: {userId}");

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            ChangeRequest changeRequest = new()
            {
                UserId = userId,
                NewValue = NewEmail,
                Password = Password,
                ChangeType = ChangeType.Email
            };

            Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(changeRequest)}");

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Settings/requestChange", changeRequest);

            Debug.WriteLine($"Response Status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                ChangeResponse? changeEmailResponse = await response.Content.ReadFromJsonAsync<ChangeResponse>();
                Debug.WriteLine($"Response Data: {JsonConvert.SerializeObject(changeEmailResponse)}");

                if (changeEmailResponse != null)
                {
                    NotificationRequest notificationRequest = new()
                    {
                        Title = "Kod weryfikacyjny",
                        Description = $"Twój kod weryfikacyjny to: {changeEmailResponse.VerificationCode}",
                        ReturningData = "VerificationCode",
                        NotificationId = 1337
                    };
                    await LocalNotificationCenter.Current.Show(notificationRequest);

                    bool verificationResult = await VerificationCodeHelper.VerifyCodeAsync(changeEmailResponse.VerificationCode);

                    if (verificationResult)
                    {
                        VerifyChangeRequest VerifyRequest = new()
                        {
                            UserId = userId,
                            VerificationCode = changeEmailResponse.VerificationCode,
                            ChangeType = ChangeType.Email
                        };

                        HttpResponseMessage verifyResponse = await client.PostAsJsonAsync("/api/Settings/verifyChange", VerifyRequest);
                        if (verifyResponse.IsSuccessStatusCode)
                        {
                            Preferences.Set("UserEmail", NewEmail);
                            await Shell.Current.DisplayAlert("Sukces", "Adres e-mail został zmieniony.", "OK");
                        }
                        else
                        {
                            ApiResponse? errorResponse = await verifyResponse.Content.ReadFromJsonAsync<ApiResponse>();
                            ErrorMessage = errorResponse?.Message ?? "Błąd podczas weryfikacji adresu e-mail. Spróbuj ponownie.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Nieprawidłowy kod weryfikacyjny.";
                    }
                }
                else
                {
                    ErrorMessage = "Błąd podczas odczytywania kodu weryfikacyjnego.";
                }
            }
            else
            {
                ApiResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                ErrorMessage = errorResponse?.Message ?? "Żądanie zmiany adresu e-mail nie powiodło się. Spróbuj ponownie.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Brak połączenia z internetem.";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error: {ex}");
            ErrorMessage = $"Wystąpił nieoczekiwany błąd: {ex.Message}. Spróbuj ponownie.";
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