using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using QRCoder;
using QrToPay.Models.Common;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

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
        try
        {
            IsBusy = true;

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
                UserId = userId,
                ServiceId = ServiceId,
                Quantity = 1,
                Tokens = Points,
                TotalPrice = formattedPrice
            };

            var token = await _qrCodeService.GenerateAndUpdateTicketAsync(updateRequest);
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

            await Shell.Current.GoToAsync("//MainPage/ActiveBiletsPage"); //TODO coś tu z busy musielibysmy przestawić żeby iść dalej
        }
        catch (Exception ex)
        {
            ErrorMessage = HttpError.HandleError(ex);
        }
/*        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas zakupu biletu", "OK");
            //ErrorMessage = HttpError.HandleGeneralError();
        }*/
        finally
        {
            IsBusy = false;
        }
    }
}