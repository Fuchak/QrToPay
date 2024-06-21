using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QrToPay.Models;

namespace QrToPay.ViewModels;

public partial class SkiResortViewModel(AppState appState, IHttpClientFactory httpClientFactory) : ViewModelBase
{

    [ObservableProperty]
    private ObservableCollection<Ticket> tickets = [];

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? cityName;

    public async Task InitializeAsync()
    {
        ResortName = appState.ResortName;
        CityName = appState.CityName;

        var client = httpClientFactory.CreateClient("ApiHttpClient");
        var response = await client.GetAsync($"/skislopes/prices?skiResortId={appState.SkiResortId}");
        response.EnsureSuccessStatusCode();

        var pricesFromApi = await response.Content.ReadFromJsonAsync<List<SkiSlopePrice>>();

        if (pricesFromApi != null)
        {
            Tickets.Clear();
            foreach (var price in pricesFromApi)
            {
                Tickets.Add(new Ticket
                {
                    Points = price.Tokens,
                    Price = price.Price
                });
            }
        }

        OnPropertyChanged(nameof(Tickets));
    }

    [RelayCommand]
    public async Task GotoSkiResortBuyAsync(Ticket ticket)
    {
        appState.Points = ticket.Points;
        appState.Price = ticket.Price;

        await Shell.Current.GoToAsync($"{nameof(SkiResortBuyPage)}");
    }
}
