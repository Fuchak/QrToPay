using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Enums;

namespace QrToPay.ViewModels.SkiResort;
public partial class SkiResortCityViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppState _appState;

    public SkiResortCityViewModel(IHttpClientFactory httpClientFactory, AppState appState)
    {
        _httpClientFactory = httpClientFactory;
        _appState = appState;
    }

    [ObservableProperty]
    private ObservableCollection<City> cities = [];

    [ObservableProperty]
    string? errorMessage;

    [RelayCommand]
    public async Task LoadCitiesAsync(EntityCategory entityType)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            //HttpResponseMessage response = await client.GetAsync("/api/SkiSlopes/cities"); Stary endpoint
            HttpResponseMessage response = await client.GetAsync($"/api/Cities?entityType={entityType}");
            response.EnsureSuccessStatusCode();

            List<City>? cities = await response.Content.ReadFromJsonAsync<List<City>>();

            if (cities != null)
            {
                Cities.Clear();
                foreach (var city in cities)
                {
                    Cities.Add(city);
                }
            }
            else
            {
                ErrorMessage = "Nie udało się pobrać listy miast.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Brak połączenia z internetem.";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unexpected error: {ex}");
            ErrorMessage = "Wystąpił nieoczekiwany błąd.";
        }
    }

    [RelayCommand]
    public async Task SelectCityAsync(City city)
    {
        _appState.EntityId = city.EntityId;
        _appState.CityName = city.CityName;

        Debug.WriteLine($"EntityId set in AppState: {_appState.EntityId}");
        await Shell.Current.GoToAsync(nameof(SkiPage));
    }
}