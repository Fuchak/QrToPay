using System.Net.Http.Json;
using Plugin.LocalNotification;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Enums;

namespace QrToPay.ViewModels.ResetPassword;
//TODO to z tym kodem widziałem gdzieś już
public partial class ResetPasswordViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ResetPasswordViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

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
                    await Shell.Current.GoToAsync("ResetPasswordConfirmPage");
                }
                else
                {
                    ErrorMessage = "Błąd podczas odczytywania kodu weryfikacyjnego.";
                }
            }
            else
            {
                ApiResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                ErrorMessage = errorResponse?.Message ?? "Błąd podczas resetowania hasła. Spróbuj ponownie.";
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