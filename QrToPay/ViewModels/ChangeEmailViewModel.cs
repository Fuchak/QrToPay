using System.Net.Http.Json;
using System.Net.Http;
using System.Diagnostics;
using QrToPay.Models.Responses;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using Plugin.LocalNotification;
using Newtonsoft.Json;

namespace QrToPay.ViewModels
{
    public partial class ChangeEmailViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
    {

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
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                {
                    ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                    return;
                }

                Debug.WriteLine($"UserId: {userId}");

                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var requestData = new ChangeEmailRequest
                {
                    UserId = userId,
                    NewEmail = NewEmail,
                    Password = Password
                };

                Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(requestData)}");

                var response = await client.PostAsJsonAsync("/settings/requestEmailChange", requestData);

                Debug.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var changeEmailResponse = await response.Content.ReadFromJsonAsync<ChangeEmailResponse>();
                    Debug.WriteLine($"Response Data: {JsonConvert.SerializeObject(changeEmailResponse)}");

                    if (changeEmailResponse != null)
                    {
                        var notification = new NotificationRequest
                        {
                            Title = "Kod weryfikacyjny",
                            Description = $"Twój kod weryfikacyjny to: {changeEmailResponse.VerificationCode}",
                            ReturningData = "VerificationCode",
                            NotificationId = 1337,
                            Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(5) // Można dostosować czas powiadomienia
                            }
                        };
                        await LocalNotificationCenter.Current.Show(notification);

                        var verificationResult = await VerifyCode(changeEmailResponse.VerificationCode);

                        if (verificationResult)
                        {
                            var verifyRequestData = new VerifyEmailRequest
                            {
                                UserId = userId,
                                VerificationCode = changeEmailResponse.VerificationCode
                            };

                            var verifyResponse = await client.PostAsJsonAsync("/settings/verifyEmailChange", verifyRequestData);
                            if (verifyResponse.IsSuccessStatusCode)
                            {
                                Preferences.Set("UserEmail", NewEmail);
                                await Shell.Current.DisplayAlert("Sukces", "Adres e-mail został zmieniony.", "OK");
                            }
                            else
                            {
                                var errorContent = await verifyResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                                ErrorMessage = errorContent?.Message ?? "Błąd podczas weryfikacji adresu e-mail. Spróbuj ponownie.";
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
                    ErrorMessage = errorContent?.Message ?? "Żądanie zmiany adresu e-mail nie powiodło się. Spróbuj ponownie.";
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
                NewEmail = string.Empty;
                Password = string.Empty;
                ErrorMessage = string.Empty;
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
}