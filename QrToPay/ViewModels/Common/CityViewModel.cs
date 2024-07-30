using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;

namespace QrToPay.ViewModels.Common;
public partial class CityViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppState _appState;

    public CityViewModel(IHttpClientFactory httpClientFactory, AppState appState)
    {
        _httpClientFactory = httpClientFactory;
        _appState = appState;
    }

    [ObservableProperty]
    private ObservableCollection<City> cities = [];

    [ObservableProperty]
    string? errorMessage;

    public string? AttractionType { get; set; }

    [RelayCommand]
    public async Task LoadCitiesAsync()
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Cities/{_appState.AttractionType}");
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

        if (_appState.AttractionType == "skislope")
        {
            await Shell.Current.GoToAsync(nameof(SkiPage));
        }
        else if (_appState.AttractionType == "funfair")
        {
            await Shell.Current.GoToAsync(nameof(FunFairPage));
        }
    }
}