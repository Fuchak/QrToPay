using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;

namespace QrToPay.ViewModels.ResetPassword;
public partial class ResetPasswordConfirmViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ResetPasswordConfirmViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    private string? emailPhone = ResetPasswordViewModel.SharedEmailOrPhone;

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    private string? password;

    [ObservableProperty]
    private string? passwordConfirm;

    [RelayCommand]
    private async Task Confirm()
    {
        try
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

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            //TODO wtf co to jest czemu tu nie ma żadnego modelu pod to może to jest git?
            var requestData = new
            {
                EmailOrPhone = EmailPhone,
                VerificationCode = Code,
                NewPassword = Password
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Auth/resetPassword", requestData);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Sukces", "Hasło zostało zaktualizowane.", "OK");
                await NavigateAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                ErrorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                     ?? "Błąd podczas resetowania hasła";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
    }
}