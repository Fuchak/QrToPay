using System.Net.Http.Json;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using QrToPay.Models.Responses;
using QrToPay.Models.Enums;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.Settings;

public partial class ChangePhoneViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ChangePhoneViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    private string? newPhoneNumber;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task RequestPhoneChange()
    {
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

        IsBusy = true;
        try
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                return;
            }

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            ChangeRequest changeRequest = new()
            {
                UserId = userId,
                NewValue = NewPhoneNumber,
                Password = Password,
                ChangeType = ChangeType.Phone
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Settings/requestChange", changeRequest);

            if (response.IsSuccessStatusCode)
            {
                ChangeResponse? changePhoneResponse = await response.Content.ReadFromJsonAsync<ChangeResponse>();

                if (changePhoneResponse != null)
                {
                    NotificationRequest request = new()
                    {
                        Title = "Kod weryfikacyjny",
                        Description = $"Twój kod weryfikacyjny to: {changePhoneResponse.VerificationCode}",
                        ReturningData = "VerificationCode",
                        NotificationId = 1337
                    };
                    await LocalNotificationCenter.Current.Show(request);

                    bool verificationResult = await VerificationCodeHelper.VerifyCodeAsync(changePhoneResponse.VerificationCode);

                    if (verificationResult)
                    {
                        VerifyChangeRequest VerifyRequest = new()
                        {
                            UserId = userId,
                            VerificationCode = changePhoneResponse.VerificationCode,
                            ChangeType = ChangeType.Phone
                        };

                        HttpResponseMessage verifyResponse = await client.PostAsJsonAsync("/api/Settings/verifyChange", VerifyRequest);
                        if (verifyResponse.IsSuccessStatusCode)
                        {
                            Preferences.Set("UserPhone", NewPhoneNumber);
                            await Shell.Current.DisplayAlert("Sukces", "Numer telefonu został zmieniony.", "OK");
                        }
                        else
                        {
                            ApiResponse? errorResponse = await verifyResponse.Content.ReadFromJsonAsync<ApiResponse>();
                            ErrorMessage = errorResponse?.Message ?? "Błąd podczas weryfikacji numeru telefonu. Spróbuj ponownie.";
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
                ErrorMessage = errorResponse?.Message ?? "Żądanie zmiany numeru telefonu nie powiodło się. Spróbuj ponownie.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Brak połączenia z internetem.";
        }
        catch (Exception)
        {
            ErrorMessage = $"Wystąpił nieoczekiwany błąd Spróbuj ponownie.";
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