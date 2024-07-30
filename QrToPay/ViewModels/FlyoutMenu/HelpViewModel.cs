using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.FlyoutMenu;
public partial class HelpViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HelpViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

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

    //TODO można to dać do bazy i z bazy to czytać i tam updatować nie w kodzie
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
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

            HelpFormRequest request = new()
            {
                UserName = UserName,
                UserEmail = UserEmail,
                Subject = SelectedSubject,
                Description = Description,
                Status = "Nowe"
            };

            Debug.WriteLine($"Request Data: {JsonConvert.SerializeObject(request)}");

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Helpform", request);

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
                ApiResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                ErrorMessage = errorResponse?.Message ?? "Wysłanie zgłoszenia nie powiodło się. Spróbuj ponownie.";
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