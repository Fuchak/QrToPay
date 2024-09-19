using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.SkiResort;
public partial class SkiResortViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly SkiResortService _skiResortService;

    public SkiResortViewModel(AppState appState, SkiResortService skiResortService)
    {
        _appState = appState;
        _skiResortService = skiResortService;
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

            var result = await _skiResortService.GetSkiResortsAsync(_appState.CityName!);

            if (result.IsSuccess && result.Data != null)
            {
                SkiResorts.Clear();
                foreach (var skiResort in result.Data)
                {
                    SkiResorts.Add(new SkiResortData
                    {
                        SkiResortId = skiResort.SkiResortId,
                        ResortName = skiResort.ResortName,
                        ImageSource = "stok.png"
                    });
                }
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }
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
        _appState.UpdateResortName(skiresort.ResortName!);

        await NavigateAsync(nameof(SkiResortPricesPage));
    }
}