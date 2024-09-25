using System.Collections.ObjectModel;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.FunFair;
public partial class FunFairCityViewModel : ViewModelBase
{
    private readonly CityService _cityService;
    private readonly AppState _appState;
    private readonly CacheService _cacheService;

    public FunFairCityViewModel(CityService cityService, AppState appState, CacheService cacheService)
    {
        _cityService = cityService;
        _appState = appState;
        _cacheService = cacheService;
    }

    [ObservableProperty]
    private ObservableCollection<City> cities = [];

    [RelayCommand]
    public async Task LoadCitiesAsync(int serviceType)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var cacheKey = CacheKeyHelper.GetCacheKey(AppDataConst.FunFairCache);
            // Ładowanie z pamięci podręcznej, jeśli istnieje
            var cachedCities = await _cacheService.LoadFromCacheAsync<IEnumerable<City>>(cacheKey);
            if (cachedCities != null)
            {
                CacheService.UpdateCollection(Cities, cachedCities);
            }

            // Pobieranie danych z API i aktualizacja, jeśli jest nowa wersja
            var result = await _cityService.GetCitiesAsync(serviceType);
            if (result.IsSuccess && result.Data != null)
            {
                if (!CacheService.AreDataEqual(Cities, result.Data))
                {
                    await _cacheService.SaveToCacheAsync(cacheKey, result.Data);
                    CacheService.UpdateCollection(Cities, result.Data);
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
    public async Task SelectCityAsync(City city)
    {
        _appState.UpdateServiceId(city.ServiceId);
        _appState.UpdateCityName(city.CityName);

        await NavigateAsync(nameof(FunFairPage));
    }
}