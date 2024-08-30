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
        if (IsBusy) return;
        //if (IsBusy) return; Może poprostu tak zamiast tego true false??? https://www.youtube.com/watch?v=ve0DFu-arD8 8:17 tak ma napisane może to git
        try
        {
            IsBusy = true;
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

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

            HelpFormRequest request = new()
            {
                UserName = UserName,
                UserEmail = UserEmail,
                Subject = SelectedSubject,
                Description = Description,
                Status = "Nowe"
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Support", request);

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
        //Może wydzielimy serwis pod iconnectivity z maui i to sprawdza czy jest internet będzie to działać lepiej?
        //https://learn.microsoft.com/pl-pl/dotnet/maui/platform-integration/communication/networking?view=net-maui-8.0&tabs=android
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}