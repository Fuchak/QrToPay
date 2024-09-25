using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortPricesViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly SkiResortService _skiResortService;
    private readonly CacheService _cacheService;

    public SkiResortPricesViewModel(AppState appState, SkiResortService skiResortService, CacheService cacheService)
    {
        _appState = appState;
        _skiResortService = skiResortService;
        _cacheService = cacheService;
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

            IsBusy = true;
            var cacheKey = CacheKeyHelper.GetCacheKey(AppDataConst.SkiResortCache, _appState.CityName!, _appState.ResortName!);
            // Krok 1: Próba załadowania danych z pamięci podręcznej, jeśli istnieją
            var cachedTickets = await _cacheService.LoadFromCacheAsync<IEnumerable<Ticket>>(cacheKey);
            if (cachedTickets != null)
            {
                CacheService.UpdateCollection(Tickets, cachedTickets);
            }

            // Krok 2: Próba pobrania danych z API i aktualizacja, jeśli są nowe dane
            var result = await _skiResortService.GetSkiResortPricesAsync(_appState.AttractionId);
            if (result.IsSuccess && result.Data != null)
            {
                var newTickets = result.Data.Select(price => new Ticket
                {
                    Points = price.Tokens,
                    Price = price.Price
                });

                if (!CacheService.AreDataEqual(Tickets, newTickets))
                {
                    await _cacheService.SaveToCacheAsync(cacheKey, newTickets);
                    CacheService.UpdateCollection(Tickets, newTickets);
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
    public async Task GotoSkiResortBuyAsync(Ticket ticket)
    {
        _appState.UpdatePoints(ticket.Points);
        _appState.UpdatePrice(ticket.Price);

        await NavigateAsync(nameof(SkiResortBuyPage));
    }
}