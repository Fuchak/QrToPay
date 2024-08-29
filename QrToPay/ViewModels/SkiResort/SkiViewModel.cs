using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;

namespace QrToPay.ViewModels.SkiResort;
public partial class SkiViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AppState _appState;
    
    public SkiViewModel(IHttpClientFactory httpClientFactory, AppState appState)
    {
        _httpClientFactory = httpClientFactory;
        _appState = appState;
    }

    [ObservableProperty]
    private ObservableCollection<SkiResortData> skiResorts = [];

    [ObservableProperty]
    string? errorMessage;

    [RelayCommand]
    public async Task LoadSkiResortsAsync()
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/resorts?serviceId={_appState.ServiceId}");
            response.EnsureSuccessStatusCode();

            List<SkiResortData>? skiResorts = await response.Content.ReadFromJsonAsync<List<SkiResortData>>();

            if (skiResorts != null)
            {
                SkiResorts.Clear();
                foreach (var skiResort in SkiResorts)
                {
                    SkiResorts.Add(new SkiResortData
                    {
                        SkiResortId = skiResort.SkiResortId,
                        ResortName = skiResort.ResortName,
                        ImageSource = "stok.png"
                    });
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
    private async Task GotoSkiResortAsync(SkiResortData skiresort)
    {

        Debug.WriteLine($"Selected SkiResortID: {skiresort.SkiResortId}, ResortName: {skiresort.ResortName}");

        _appState.AttractionId = skiresort.SkiResortId;
        _appState.ResortName = skiresort.ResortName;

        await Shell.Current.GoToAsync($"{nameof(SkiResortPage)}");
    }
}