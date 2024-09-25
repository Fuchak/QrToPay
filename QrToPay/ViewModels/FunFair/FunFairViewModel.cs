using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using QrToPay.View.FunFair;

namespace QrToPay.ViewModels.FunFair;
public partial class FunFairViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly FunFairService _funFairService;
    private readonly CacheService _cacheService;

    public FunFairViewModel(AppState appState, FunFairService funFairService, CacheService cacheService)
    {
        _appState = appState;
        _funFairService = funFairService;
        _cacheService = cacheService;
    }

    [ObservableProperty]
    private ObservableCollection<FunFairData> funFairs = [];

    [RelayCommand]
    public async Task LoadFunFairsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            var cacheKey = CacheKeyHelper.GetCacheKey(AppDataConst.FunFairCache, _appState.CityName!);

            var cachedFunFairs = await _cacheService.LoadFromCacheAsync<IEnumerable<FunFairData>>(cacheKey);
            if (cachedFunFairs != null)
            {
                CacheService.UpdateCollection(FunFairs, cachedFunFairs);
            }

            var result = await _funFairService.GetFunFairsAsync(_appState.CityName!);
            if (result.IsSuccess && result.Data != null)
            {
                var newFunFairs = result.Data.Select(funFair => new FunFairData
                {
                    FunFairId = funFair.FunFairId,
                    ResortName = funFair.ResortName,
                    ImageSource = AppDataConst.FunFairPNG
                });

                if (!CacheService.AreDataEqual(FunFairs, newFunFairs))
                {
                    await _cacheService.SaveToCacheAsync(cacheKey, newFunFairs);
                    CacheService.UpdateCollection(FunFairs, newFunFairs);
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
    private async Task GotoFunFairAsync(FunFairData funFair)
    {
        _appState.UpdateAttractionId(funFair.FunFairId);
        _appState.UpdateResortName(funFair.ResortName!);

        await NavigateAsync(nameof(FunFairPricesPage));
    }
}