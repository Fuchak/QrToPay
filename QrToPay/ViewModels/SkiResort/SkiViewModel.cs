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

    [RelayCommand]
    public async Task LoadSkiResortsAsync()
    {
        if(IsBusy) return;
        try
        {
            IsBusy = true;
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/resorts?cityName={_appState.CityName}");
            response.EnsureSuccessStatusCode();

            List<SkiResortData>? skiResortsData = await response.Content.ReadFromJsonAsync<List<SkiResortData>>();

            if (skiResortsData != null)
            {
                SkiResorts.Clear();
                foreach (var skiResort in skiResortsData)
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
    private async Task GotoSkiResortAsync(SkiResortData skiresort)
    {
        _appState.UpdateAttractionId(skiresort.SkiResortId);
        _appState.UpdateResortName(skiresort.ResortName);

        await NavigateAsync(nameof(SkiResortPage));
    }
}