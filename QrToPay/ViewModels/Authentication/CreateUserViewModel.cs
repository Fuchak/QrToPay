using System.Net.Http.Json;
using Plugin.LocalNotification;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.Authentication
{
    public partial class CreateUserViewModel : ViewModelBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateUserViewModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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
                HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

                object request;

                if (ValidationHelper.IsEmail(EmailPhone))
                {
                    request = new CreateUserRequest { Email = EmailPhone, PasswordHash = Password };
                }
                else if (ValidationHelper.IsPhoneNumber(EmailPhone))
                {
                    request = new CreateUserRequest { PhoneNumber = EmailPhone, PasswordHash = Password };
                }
                else
                {
                    ErrorMessage = "Podaj poprawny email lub numer telefonu.";
                    IsBusy = false;
                    return;
                }

                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Register/register", request);

                if (response.IsSuccessStatusCode)
                {
                    CreateUserResponse? registerResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
                    
                    if (registerResponse != null)
                    {
                        NotificationRequest notification = new()
                        {
                            Title = "Kod weryfikacyjny",
                            Description = $"Twój kod weryfikacyjny to: {registerResponse.VerificationCode}",
                            ReturningData = "VerificationCode",
                            NotificationId = 1337
                        };
                        await LocalNotificationCenter.Current.Show(notification);

                        bool verificationResult = await VerificationCodeHelper.VerifyCodeAsync(registerResponse.VerificationCode);

                        if (verificationResult)
                        {
                            VerifyCreateUserRequest verifyData = new()
                            {
                                EmailOrPhone = registerResponse.EmailOrPhone,
                                VerificationCode = registerResponse.VerificationCode
                            };

                            HttpResponseMessage verifyResponse = await client.PostAsJsonAsync("/api/Register/verify", verifyData);

                            if (verifyResponse.IsSuccessStatusCode)
                            {
                                await Shell.Current.DisplayAlert("Sukces", "Kod weryfikacyjny jest poprawny.", "OK");
                                await Shell.Current.GoToAsync("//LoginPage");
                            }
                            else
                            {
                                ApiResponse? errorResponse = await verifyResponse.Content.ReadFromJsonAsync<ApiResponse>();
                                ErrorMessage = errorResponse?.Message ?? "Błąd podczas weryfikacji użytkownika. Spróbuj ponownie.";
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
                    ErrorMessage = errorResponse?.Message ?? "Rejestracja nie powiodła się. Spróbuj ponownie.";
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
    }
}