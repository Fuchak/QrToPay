using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using QRCoder;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using QrToPay.View;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortBuyViewModel : QuantityViewModelBase
{
    private readonly TicketService _ticketService;
    private readonly QrCodeStorageService _qrCodeStorageService;
    private readonly AppState _appState;
    private readonly BalanceService _balanceService;

    public SkiResortBuyViewModel(TicketService ticketService, QrCodeStorageService qrCodeStorageService, AppState appState, BalanceService balanceService)
    {
        _ticketService = ticketService;
        _qrCodeStorageService = qrCodeStorageService;
        _appState = appState;
        _balanceService = balanceService;

        Price = _appState.Price;
        Points = _appState.Points;
    }

    [ObservableProperty]
    private ObservableCollection<Ticket> tickets = [];

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? cityName;

    [ObservableProperty]
    private string? biletType;

    [ObservableProperty]
    private int serviceId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBuying))]
    bool isBuying;

    public bool IsNotBuying => !IsBuying;

    public Task InitializeAsync()
    {
        ResortName = _appState.ResortName;
        ServiceId = _appState.ServiceId;
        CityName = _appState.CityName;

        return Task.CompletedTask;
    }

    protected override void UpdatePriceAndPoints()
    {
        Price = _appState.Price * Quantity;
        Points = _appState.Points * Quantity;
    }

    [RelayCommand]
    private async Task GenerateQrCodeAsync()
    {
        if (IsBuying) return;
        try
        {
            IsBuying = true;

            if (string.IsNullOrEmpty(ResortName) || string.IsNullOrEmpty(CityName))
            {
                await Shell.Current.DisplayAlert("Błąd", "Wszystkie pola muszą być wypełnione", "OK");
                return;
            }
            string formattedPrice = Price.ToString(CultureInfo.InvariantCulture);

            var balanceResult = await _balanceService.LoadUserDataAsync();
            if (!balanceResult.IsSuccess)
            {
                ErrorMessage = balanceResult.ErrorMessage;
                return;
            }

            if (balanceResult.Data < Price)
            {
                ErrorMessage = "Nie masz wystarczających środków na koncie.";
                return;
            }

            UpdateTicketRequest updateRequest = new()
            {
                ServiceId = ServiceId,
                Quantity = Quantity,
                Tokens = Points,
                TotalPrice = formattedPrice
            };

            var result = await _ticketService.GenerateAndUpdateTicketAsync(updateRequest);

            if (!result.IsSuccess)
            {
                ErrorMessage = result.ErrorMessage;
                return;
            }

            string token = result.Data!;

            Ticket newTicket = new()
            {
                EntityName = ResortName,
                CityName = CityName,
                Price = Price,
                Points = Points,
                QrCode = token
            };

            Tickets.Add(newTicket);

            await _qrCodeStorageService.GenerateAndSaveQrCodeImageAsync(token);

            await Shell.Current.DisplayAlert("Potwierdzenie", "Bilet został zakupiony, kod QR wygenerowany", "OK");

            await NavigateAsync($"//{nameof(MainPage)}/{nameof(ActiveBiletsPage)}");
        }
        finally
        {
            IsBuying = false;
        }
    }
}