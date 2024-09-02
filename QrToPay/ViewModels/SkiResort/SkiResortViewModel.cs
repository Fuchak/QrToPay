using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly IHttpClientFactory _httpClientFactory;

    public SkiResortViewModel(AppState appState, IHttpClientFactory httpClientFactory)
    {
        _appState = appState;
        _httpClientFactory = httpClientFactory;
    }
    [ObservableProperty]
    private ObservableCollection<Ticket> tickets = [];

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? cityName;

    public async Task InitializeAsync()
    {
        if (IsBusy) return;
        try
        {
            ResortName = _appState.ResortName;
            CityName = _appState.CityName;

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/prices?skiResortId={_appState.AttractionId}");
            response.EnsureSuccessStatusCode();

            List<SkiResortPriceResponse>? skiResortPrices = await response.Content.ReadFromJsonAsync<List<SkiResortPriceResponse>>();

            if (skiResortPrices != null)
            {
                Tickets.Clear();
                foreach (var price in skiResortPrices)
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
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
        finally
        {
            IsBusy = false; // Resetowanie stanu po zakończeniu operacji
        }
    }

    //TODO gdzie tu jest jakiś try catch nic nie ma? xd

    [RelayCommand]
    public async Task GotoSkiResortBuyAsync(Ticket ticket)
    {
        _appState.UpdatePoints(ticket.Points);
        _appState.UpdatePrice(ticket.Price);

        await Shell.Current.GoToAsync($"{nameof(SkiResortBuyPage)}");
    }
}