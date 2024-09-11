using System.Collections.ObjectModel;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;
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

            var result = await _skiResortService.GetSkiResortPricesAsync(_appState.AttractionId);

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
            //OnPropertyChanged(nameof(Tickets));
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
    public async Task GotoSkiResortBuyAsync(Ticket ticket)
    {
        _appState.UpdatePoints(ticket.Points);
        _appState.UpdatePrice(ticket.Price);

        await NavigateAsync(nameof(SkiResortBuyPage));
    }
}