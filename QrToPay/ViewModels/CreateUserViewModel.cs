using System.Net.Http.Json;
using System.Net.Http;
using System.Text.RegularExpressions;
using Plugin.LocalNotification;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using Newtonsoft.Json;

namespace QrToPay.ViewModels;

public partial class CreateUserViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [ObservableProperty]
    private string? emailPhone;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Confirm()
    {
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

        IsBusy = true;
        try
        {
            var client = httpClientFactory.CreateClient("ApiHttpClient");

            string userType;
            object requestData;


            if (ValidationHelper.IsEmail(EmailPhone))
            {
                userType = "email";
                requestData = new { Email = EmailPhone, Password };
            }
            else if (ValidationHelper.IsPhoneNumber(EmailPhone))
            {
                userType = "phone";
                requestData = new { PhoneNumber = EmailPhone, Password };
            }
            else
            {
                ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                IsBusy = false;
                return;
            }

            var response = await client.PostAsJsonAsync($"/register/{userType}", requestData);

            if (response.IsSuccessStatusCode)
            {
                // Pobierz kod weryfikacyjny
                var registerResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>();

                if (registerResponse != null)
                {
                    // Wyślij powiadomienie z kodem weryfikacyjnym
                    var notification = new NotificationRequest
                    {
                        Title = "Kod weryfikacyjny",
                        Description = $"Twój kod weryfikacyjny to: {registerResponse.VerificationCode}",
                        ReturningData = "VerificationCode",
                        NotificationId = 1337,
                        Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(5) // Można dostosować czas powiadomienia
                            }
                    };
                    await LocalNotificationCenter.Current.Show(notification);

                    var verificationResult = await VerifyCode(registerResponse.VerificationCode);

                    if (verificationResult)
                    {
                        var verifyRequestData = new CreateUserResponse
                        {
                            EmailOrPhone = EmailPhone,
                            VerificationCode = registerResponse.VerificationCode
                        };

                        var verifyResponse = await client.PostAsJsonAsync("/register/verify", verifyRequestData);
                        if (verifyResponse.IsSuccessStatusCode)
                        {
                            await Shell.Current.DisplayAlert("Sukces", "Kod weryfikacyjny jest poprawny.", "OK");
                            await Shell.Current.GoToAsync("//LoginPage");
                        }
                        else
                        {
                            var errorContent = await verifyResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                            ErrorMessage = errorContent?.Message ?? "Błąd podczas weryfikacji użytkownika. Spróbuj ponownie.";
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
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorMessage = errorContent?.Message ?? "Rejestracja nie powiodła się. Spróbuj ponownie.";
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
            Password = string.Empty;
            PasswordConfirm = string.Empty;
        }
    }

    private async Task<bool> VerifyCode(string? actualCode)
    {
        var popupViewModel = new VerificationCodePopupViewModel();
        var result = await Shell.Current.ShowPopupAsync(new VerificationCodePopup(popupViewModel));
        if (result is not null)
        {
            var enteredCode = result.ToString();
            return string.Equals(enteredCode, actualCode, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}
