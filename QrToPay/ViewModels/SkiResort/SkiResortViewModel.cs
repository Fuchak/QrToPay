using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.SkiResort;
public partial class SkiResortViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly SkiResortService _skiResortService;
    private readonly CacheService _cacheService;

    public SkiResortViewModel(AppState appState, SkiResortService skiResortService, CacheService cacheService)
    {
        _appState = appState;
        _skiResortService = skiResortService;
        _cacheService = cacheService;
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
            var cacheKey = CacheKeyHelper.GetCacheKey(AppDataConst.SkiResortCache, _appState.CityName!);

            var cachedResorts = await _cacheService.LoadFromCacheAsync<IEnumerable<SkiResortData>>(cacheKey);
            if (cachedResorts != null)
            {
                CacheService.UpdateCollection(SkiResorts, cachedResorts);
            }

            var result = await _skiResortService.GetSkiResortsAsync(_appState.CityName!);
            if (result.IsSuccess && result.Data != null)
            {
                var newSkiResorts = result.Data.Select(skiResort => new SkiResortData
                {
                    SkiResortId = skiResort.SkiResortId,
                    ResortName = skiResort.ResortName,
                    ImageSource = AppDataConst.SkiResortPNG
                });

                if (!CacheService.AreDataEqual(SkiResorts, newSkiResorts))
                {
                    await _cacheService.SaveToCacheAsync(cacheKey, newSkiResorts);
                    CacheService.UpdateCollection(SkiResorts, newSkiResorts);
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