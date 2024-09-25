using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using QrToPay.View.FunFair;

namespace QrToPay.ViewModels.FunFair;

public partial class FunFairPricesViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly FunFairService _funFairService;
    private readonly CacheService _cacheService;

    public FunFairPricesViewModel(AppState appState, FunFairService funFairService, CacheService cacheService)
    {
        _appState = appState;
        _funFairService = funFairService;
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

            var cacheKey = CacheKeyHelper.GetCacheKey(AppDataConst.FunFairCache, _appState.CityName!, _appState.ResortName!);

            var cachedTickets = await _cacheService.LoadFromCacheAsync<IEnumerable<Ticket>>(cacheKey);
            if (cachedTickets != null)
            {
                CacheService.UpdateCollection(Tickets, cachedTickets);
            }

            var result = await _funFairService.GetFunFairPricesAsync(_appState.AttractionId);
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
    public async Task GotoFunFairBuyAsync(Ticket ticket)
    {
        _appState.UpdatePoints(ticket.Points);
        _appState.UpdatePrice(ticket.Price);

        await NavigateAsync(nameof(FunFairBuyPage));
    }
}