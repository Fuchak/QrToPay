using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Animations;
using QrToPay.Models;

namespace QrToPay.ViewModels;

public partial class SkiResortBuyViewModel(QrCodeService qrCodeService, AppState appState, BalanceService balanceService) : ViewModelBase
{
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
    private int skiResortId;

    [ObservableProperty]
    private int funFairId;

    private int userId;

    public Task InitializeAsync()
    {
        ResortName = appState.ResortName;
        FunFairId = appState.FunFairId;
        SkiResortId = appState.SkiResortId;
        CityName = appState.CityName;
        Price = appState.Price;
        Points = appState.Points;
        userId = Preferences.Get("UserId", 0);

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task GenerateQrCodeAsync()
    {
        if (string.IsNullOrEmpty(ResortName) || string.IsNullOrEmpty(CityName))
        {
            await Shell.Current.DisplayAlert("Błąd", "Wszystkie pola muszą być wypełnione", "OK");
            return;
        }
            string formattedPrice = Price.ToString(CultureInfo.InvariantCulture);
        try
        {
            var (accountBalance, errorMessage) = await balanceService.LoadUserDataAsync();
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

            var updateRequest = new UpdateTicketRequest
            {
                UserID = userId,
                SkiResortID = SkiResortId == 0 ? (int?)null : SkiResortId,
                FunFairID = FunFairId == 0 ? (int?)null : FunFairId,
                Quantity = 1,
                Tokens = Points,
                TotalPrice = formattedPrice
            };

            Debug.WriteLine($"Sending Update Request: UserID={updateRequest.UserID}, SkiResortID={updateRequest.SkiResortID}, FunFairID={updateRequest.FunFairID}, Quantity={updateRequest.Quantity}, Tokens={updateRequest.Tokens}, TotalPrice={updateRequest.TotalPrice}");

            var qrCode = await qrCodeService.GenerateAndUpdateTicketAsync(updateRequest);
            if (string.IsNullOrEmpty(qrCode))
            {
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się zaktualizować bazy danych", "OK");
                return;
            }

            var newTicket = new Ticket
            {
                UserId = userId,
                ResortName = ResortName,
                CityName = CityName,
                Price = Price,
                Points = Points,
                QrCode = qrCode
            };

            Tickets.Add(newTicket);

            await Shell.Current.DisplayAlert("Potwierdzenie", "Bilet został zakupiony, kod QR wygenerowany", "OK");

            await Shell.Current.GoToAsync($"//MainPage/ActiveBiletsPage");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Błąd przy generowaniu kodu qr lub aktualizowaniu bazy: {ex.Message}");
            await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas zakupu biletu", "OK");
        }
    }
}