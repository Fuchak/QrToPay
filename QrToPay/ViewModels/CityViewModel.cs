using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QrToPay.Models;

namespace QrToPay.ViewModels
{
    public partial class CityViewModel(IHttpClientFactory httpClientFactory, AppState appState) : ViewModelBase
    {

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
                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var response = await client.GetAsync("/cities");
                response.EnsureSuccessStatusCode();

                var citiesFromApi = await response.Content.ReadFromJsonAsync<List<City>>();

                if (citiesFromApi != null)
                {
                    Cities.Clear();
                    foreach (var city in citiesFromApi)
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
            appState.CityId = city.CityId;
            appState.CityName = city.CityName;

            if (appState.AttractionType == "skislope")
            {
                await Shell.Current.GoToAsync(nameof(SkiPage));
            }
            else if (appState.AttractionType == "funfair")
            {
                await Shell.Current.GoToAsync(nameof(FunFairPage));
            }
        }
    }
}
