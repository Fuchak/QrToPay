using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Android.OS;
using Kotlin.Coroutines.Jvm.Internal;
using QRCoder;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;
using QrToPay.Services.Local;
using QrToPay.View;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortBuyViewModel : ViewModelBase
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
    private decimal price;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private int points;

    [ObservableProperty]
    private Guid serviceId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBuying))]
    bool isBuying;

    public bool IsNotBuying => !IsBuying;

    private int userId;



    private bool _isButtonPressed;
    private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(200);
    private int _buttonDirection;

    public Task InitializeAsync()
    {
        ResortName = _appState.ResortName;
        ServiceId = _appState.ServiceId;
        CityName = _appState.CityName;
        Price = _appState.Price;
        Points = _appState.Points;
        userId = Preferences.Get("UserId", 0);

        return Task.CompletedTask;
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

            int userId = Preferences.Get("UserId", 0);

            var balanceResult = await _balanceService.LoadUserDataAsync(userId);
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
                UserId = userId,
                ServiceId = ServiceId,
                Quantity = Quantity,
                Tokens = Points,
                TotalPrice = formattedPrice
            };

            var token = await _ticketService.GenerateAndUpdateTicketAsync(updateRequest);
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Nie udało się przetworzyć płatności";
                return;
            }

            Ticket newTicket = new()
            {
                UserId = userId,
                EntityName = ResortName,
                CityName = CityName,
                Price = Price,
                Points = Points,
                QrCode = token
            };

            Tickets.Add(newTicket);

            await _qrCodeStorageService.GenerateAndSaveQrCodeImageAsync(userId, token);
            
            await Shell.Current.DisplayAlert("Potwierdzenie", "Bilet został zakupiony, kod QR wygenerowany", "OK");

            await NavigateAsync($"//{nameof(MainPage)}/{nameof(ActiveBiletsPage)}");
        }
        finally
        {
            IsBuying = false;
        }
    }

    [RelayCommand]
    private void IncreaseQuantity()
    {
        if (Quantity < 999999)
        {
            Quantity++;
            UpdatePriceAndPoints();
        }
        else
        {
            ErrorMessage = "Osiągnięto maksymalna ilość biletów.";
        }
    }

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
        {
            Quantity--;
            UpdatePriceAndPoints();
        }
    }

    private void UpdatePriceAndPoints()
    {
        Price = _appState.Price * Quantity;
        Points = _appState.Points * Quantity;
    }

    public void StartRepeatingAction(int direction)
    {
        _buttonDirection = direction;
        _isButtonPressed = true;

        Shell.Current.Dispatcher.StartTimer(_interval, () =>
        {
            if (!_isButtonPressed) { return false; }

            if (_buttonDirection == 1) { IncreaseQuantity(); }
               
            else if (_buttonDirection == -1) { DecreaseQuantity(); }
                
            return true;
        });
    }

    public void StopRepeatingAction()
    {
        _isButtonPressed = false;
    }
}