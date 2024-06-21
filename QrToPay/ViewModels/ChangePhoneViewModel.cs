using System.Net.Http.Json;
using System.Net.Http;
using System.Diagnostics;
using QrToPay.Helpers;
using QrToPay.Models.Responses;
using CommunityToolkit.Maui.Views;
using Plugin.LocalNotification;
using Newtonsoft.Json;

namespace QrToPay.ViewModels
{
    public partial class ChangePhoneViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
    {
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
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                {
                    ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                    return;
                }

                Debug.WriteLine($"UserId: {userId}");

                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var requestData = new ChangePhoneRequest
                {
                    UserId = userId,
                    NewPhoneNumber = NewPhoneNumber,
                    Password = Password
                };

                Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(requestData)}");

                var response = await client.PostAsJsonAsync("/settings/requestPhoneChange", requestData);

                Debug.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var changePhoneResponse = await response.Content.ReadFromJsonAsync<ChangePhoneResponse>();
                    Debug.WriteLine($"Response Data: {JsonConvert.SerializeObject(changePhoneResponse)}");

                    if (changePhoneResponse != null)
                    {
                        var notification = new NotificationRequest
                        {
                            Title = "Kod weryfikacyjny",
                            Description = $"Twój kod weryfikacyjny to: {changePhoneResponse.VerificationCode}",
                            ReturningData = "VerificationCode",
                            NotificationId = 1338,
                            Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(5) // Można dostosować czas powiadomienia
                            }
                        };
                        await LocalNotificationCenter.Current.Show(notification);

                        var verificationResult = await VerifyCode(changePhoneResponse.VerificationCode);

                        if (verificationResult)
                        {
                            var verifyRequestData = new VerifyPhoneRequest
                            {
                                UserId = userId,
                                VerificationCode = changePhoneResponse.VerificationCode
                            };

                            var verifyResponse = await client.PostAsJsonAsync("/settings/verifyPhoneChange", verifyRequestData);
                            if (verifyResponse.IsSuccessStatusCode)
                            {
                                Preferences.Set("UserPhone", NewPhoneNumber);
                                await Shell.Current.DisplayAlert("Sukces", "Numer telefonu został zmieniony.", "OK");
                            }
                            else
                            {
                                var errorContent = await verifyResponse.Content.ReadFromJsonAsync<ErrorResponse>();
                                ErrorMessage = errorContent?.Message ?? "Błąd podczas weryfikacji numeru telefonu. Spróbuj ponownie.";
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
                    ErrorMessage = errorContent?.Message ?? "Żądanie zmiany numeru telefonu nie powiodło się. Spróbuj ponownie.";
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
                NewPhoneNumber = string.Empty;
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
