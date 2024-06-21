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
    public partial class SkiViewModel(IHttpClientFactory httpClientFactory, AppState appState) : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<SkiSlope> skislopes = [];

        [ObservableProperty]
        string? errorMessage;

        [RelayCommand]
        public async Task LoadSkiResortsAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var response = await client.GetAsync($"/skislopes/slopes?cityId={appState.CityId}");
                response.EnsureSuccessStatusCode();

                var skiSlopesFromApi = await response.Content.ReadFromJsonAsync<List<SkiSlope>>();

                if (skiSlopesFromApi != null)
                {
                    Skislopes.Clear();
                    foreach (var skiResort in skiSlopesFromApi)
                    {
                        skiResort.ImageSource = "stok.png";
                        Skislopes.Add(skiResort);
                    }
                }
                else
                {
                    ErrorMessage = "Nie udało się pobrać danych ze stoku narciarskiego.";
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
        public async Task GotoSkiResortAsync(SkiSlope skiresort)
        {
            appState.SkiResortId = skiresort.SkiResortId;
            appState.ResortName = skiresort.ResortName;
            await Shell.Current.GoToAsync($"{nameof(SkiResortPage)}");
        }
    }
}