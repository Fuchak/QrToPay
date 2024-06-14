using System.Diagnostics;
using System.Net.Http.Json;

namespace QrToPay.ViewModels;

public partial class ResetPasswordConfirmViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    private string? emailPhone = ResetPasswordViewModel.SharedEmailOrPhone;

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task Confirm()
    {
        if (string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordConfirm))
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

            var requestData = new
            {
                EmailOrPhone = EmailPhone,
                VerificationCode = Code,
                NewPassword = Password
            };

            var response = await client.PostAsJsonAsync("/resetpassword/resetpassword", requestData);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zaktualizowane.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorMessage = errorContent?.Message ?? "Błąd podczas resetowania hasła.";
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
        }
    }
}
