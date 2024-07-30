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
    private ObservableCollection<SkiSlope> skislopes = [];

    [ObservableProperty]
    string? errorMessage;

    [RelayCommand]
    public async Task LoadSkiResortsAsync()
    {
        try
        {
            Debug.WriteLine($"EntityId used for fetching ski slopes: {_appState.EntityId}");

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiSlopes/slopes?entityId={_appState.EntityId}"); // tu było cityname
            response.EnsureSuccessStatusCode();

            List<SkiSlope>? skiSlopes = await response.Content.ReadFromJsonAsync<List<SkiSlope>>();

            if (skiSlopes != null)
            {
                Skislopes.Clear();
                foreach (var skiResort in skiSlopes)
                {
                    Skislopes.Add(new SkiSlope
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
    private async Task GotoSkiResortAsync(SkiSlope skiresort)
    {

        Debug.WriteLine($"Selected SkiResortID: {skiresort.SkiResortId}, ResortName: {skiresort.ResortName}");

        _appState.AttractionId = skiresort.SkiResortId;
        _appState.ResortName = skiresort.ResortName;

        await Shell.Current.GoToAsync($"{nameof(SkiResortPage)}");
    }
}