using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;
using QrToPay.Services.Api;
using QrToPay.View.FunFair;

namespace QrToPay.ViewModels.FunFair;

public partial class FunFairPricesViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly FunFairService _funFairService;

    public FunFairPricesViewModel(AppState appState, FunFairService funFairService)
    {
        _appState = appState;
        _funFairService = funFairService;
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
            var result = await _funFairService.GetFunFairPricesAsync(_appState.AttractionId);

            if (result.IsSuccess && result.Data != null)
            {
                Tickets.Clear();
                foreach (var price in result.Data)
                {
                    Tickets.Add(new Ticket
                    {
                        Points = price.Tokens,
                        Price = price.Price
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
    public async Task GotoFunFairBuyAsync(Ticket ticket)
    {
        _appState.UpdatePoints(ticket.Points);
        _appState.UpdatePrice(ticket.Price);

        await NavigateAsync(nameof(FunFairBuyPage));
    }
}