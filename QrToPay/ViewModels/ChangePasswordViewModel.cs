using System.Net.Http.Json;
using System.Net.Http;
using System.Diagnostics;
using QrToPay.Models.Responses;
using Newtonsoft.Json;

namespace QrToPay.ViewModels
{
    public partial class ChangePasswordViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
    {
        [ObservableProperty]
        private string? oldPassword;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private string? passwordConfirm;

        [ObservableProperty]
        private string? errorMessage;

        [RelayCommand]
        private async Task Confirm()
        {
            if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
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
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                {
                    ErrorMessage = "Nie można znaleźć identyfikatora użytkownika.";
                    return;
                }

                Debug.WriteLine($"UserId: {userId}");

                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var requestData = new ChangePasswordRequest
                {
                    UserId = userId,
                    OldPassword = OldPassword,
                    NewPassword = Password,
                    ConfirmNewPassword = PasswordConfirm
                };

                Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(requestData)}");

                var response = await client.PostAsJsonAsync("/settings/changePassword", requestData);

                Debug.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zmienione.", "OK");
                }
                else
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    ErrorMessage = errorContent?.Message ?? "Zmiana hasła nie powiodła się. Spróbuj ponownie.";
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
                OldPassword = string.Empty;
                Password = string.Empty;
                PasswordConfirm = string.Empty;
            }
        }
    }
}
