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
    public async Task LoadCitiesAsync(int serviceType)
    {
        if(IsBusy) return;
        try
        {
            IsBusy = true;
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Cities?serviceType={serviceType}");
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
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SelectCityAsync(City city)
    {
        _appState.UpdateServiceId(city.ServiceId);
        _appState.UpdateCityName(city.CityName);

        await Shell.Current.GoToAsync(nameof(SkiPage));
    }
}