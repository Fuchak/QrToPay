using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using QRCoder;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.SkiResort;

public partial class SkiResortBuyViewModel : ViewModelBase
{

    private readonly QrCodeService _qrCodeService;
    private readonly QrCodeStorageService _qrCodeStorageService;
    private readonly AppState _appState;
    private readonly BalanceService _balanceService;

    public SkiResortBuyViewModel(QrCodeService qrCodeService, QrCodeStorageService qrCodeStorageService, AppState appState, BalanceService balanceService)
    {
        _qrCodeService = qrCodeService;
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
    private int points;

    [ObservableProperty]
    private Guid serviceId;

    [ObservableProperty]
    private bool isBusy;

    private int userId;

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
        if (IsBusy) return; // Zapobiega wielokrotnemu kliknięciu
        IsBusy = true;

        if (string.IsNullOrEmpty(ResortName) || string.IsNullOrEmpty(CityName))
        {
            await Shell.Current.DisplayAlert("Błąd", "Wszystkie pola muszą być wypełnione", "OK");
            return;
        }
            string formattedPrice = Price.ToString(CultureInfo.InvariantCulture);
        try
        {
            var (accountBalance, errorMessage) = await _balanceService.LoadUserDataAsync();
            if (accountBalance == null)
            {
                await Shell.Current.DisplayAlert("Błąd", errorMessage, "OK");
                return;
            }

            if (accountBalance < Price)
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie masz wystarczających środków na koncie.", "OK");
                return;
            }

            UpdateTicketRequest updateRequest = new()
            {
                UserId = userId,
                ServiceId = ServiceId,
                Quantity = 1,
                Tokens = Points,
                TotalPrice = formattedPrice
            };
            //TODO usun debug potem xd
            Debug.WriteLine($"Sending Update Request: UserID={updateRequest.UserId}, ServiceId={updateRequest.ServiceId}, Quantity={updateRequest.Quantity}, Tokens={updateRequest.Tokens}, TotalPrice={updateRequest.TotalPrice}");


            var token = await _qrCodeService.GenerateAndUpdateTicketAsync(updateRequest);
            if (string.IsNullOrEmpty(token))
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się zaktualizować bazy danych", "OK");
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

            await Shell.Current.GoToAsync($"//MainPage/ActiveBiletsPage");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Błąd przy generowaniu kodu qr lub aktualizowaniu bazy: {ex.Message}");
            await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas zakupu biletu", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}