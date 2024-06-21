using System.Net.Http.Json;
using System.Net.Http;
using Plugin.LocalNotification;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;

namespace QrToPay.ViewModels;

public partial class ResetPasswordViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    public static string? SharedEmailOrPhone { get; private set; }

    [ObservableProperty]
    private string? emailPhone;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Confirm()
    {
        if (string.IsNullOrWhiteSpace(EmailPhone))
        {
            ErrorMessage = "Podaj email lub numer telefonu.";
            return;
        }

        IsBusy = true;
        try
        {
            var client = httpClientFactory.CreateClient("ApiHttpClient");

            string userType;
            object requestData;

            if (ValidationHelper.IsEmail(EmailPhone))
            {
                userType = "email";
                requestData = new { Email = EmailPhone };
            }
            else if (ValidationHelper.IsPhoneNumber(EmailPhone))
            {
                userType = "phone";
                requestData = new { PhoneNumber = EmailPhone };
            }
            else
            {
                ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                IsBusy = false;
                return;
            }

            var response = await client.PostAsJsonAsync($"/resetpassword/{userType}", requestData);
            if (response.IsSuccessStatusCode)
            {
                var resetResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
                if (resetResponse != null)
                {
                    var notification = new NotificationRequest
                    {
                        Title = "Kod weryfikacyjny",
                        Description = $"Twój kod weryfikacyjny to: {resetResponse.VerificationCode}",
                        ReturningData = "VerificationCode",
                        NotificationId = 1337,
                        Schedule =
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5) // Można dostosować czas powiadomienia
                    }
                    };
                    await LocalNotificationCenter.Current.Show(notification);

                    SharedEmailOrPhone = EmailPhone;
                    await Shell.Current.GoToAsync("ResetPasswordConfirmPage");
                }
                else
                {
                    ErrorMessage = "Błąd podczas odczytywania kodu weryfikacyjnego.";
                }
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorMessage = errorContent?.Message ?? "Błąd podczas resetowania hasła. Spróbuj ponownie.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Brak połączenia z internetem.";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error: {ex.Message}");
            ErrorMessage = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie.";
        }
        finally
        {
            IsBusy = false;
            EmailPhone = string.Empty;
        }
    }

}