using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.Http;
using QrToPay.Models.Responses;

namespace QrToPay.ViewModels;
public partial class HelpViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? problem;

    [ObservableProperty]
    private string? subject;

    [ObservableProperty]
    private string? userName;

    [ObservableProperty]
    private string? userEmail;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? selectedSubject;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<string> Subjects { get; } =
        [
            "Problem z logowaniem",
            "Błąd płatności",
            "Weryfikacja konta",
            "Inne"
        ];

    [RelayCommand]
    private async Task Submit()
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(SelectedSubject))
        {
            ErrorMessage = "Wszystkie pola są wymagane.";
            return;
        }

        if (!ValidationHelper.IsEmail(UserEmail))
        {
            ErrorMessage = "Podaj poprawny adres e-mail.";
            return;
        }

        IsBusy = true;
        try
        {
            var client = httpClientFactory.CreateClient("ApiHttpClient");
            var requestData = new HelpFormRequest
            {
                UserName = UserName,
                UserEmail = UserEmail,
                Subject = SelectedSubject,
                Description = Description,
                Status = "Nowe"
            };

            Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(requestData)}");

            var response = await client.PostAsJsonAsync("/helpform", requestData);

            Debug.WriteLine($"Response Status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Sukces", "Twoje zgłoszenie zostało wysłane.", "OK");
                // Czyszczenie formularza
                UserName = string.Empty;
                UserEmail = string.Empty;
                Description = string.Empty;
                SelectedSubject = null;
                ErrorMessage = null;
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                ErrorMessage = errorContent?.Message ?? "Wysłanie zgłoszenia nie powiodło się. Spróbuj ponownie.";
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
        }
    }
}