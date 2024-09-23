using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QRCoder;
using CommunityToolkit.Maui.Views;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class ActiveBiletsViewModel : ViewModelBase
{
    private readonly TicketService _ticketService;
    private readonly QrCodeStorageService _qrCodeStorageService;
    private readonly QrCodeService _qrCodeService;

    public ActiveBiletsViewModel(TicketService ticketService, QrCodeStorageService qrCodeStorageService, QrCodeService qrCodeService)
    {
        _ticketService = ticketService;
        _qrCodeStorageService = qrCodeStorageService;
        _qrCodeService = qrCodeService;
    }

    [ObservableProperty]
    private ObservableCollection<Ticket> activeTickets = [];

    [ObservableProperty]
    private bool isQrCodeVisible;

    [ObservableProperty]
    private string? resortName;

    [ObservableProperty]
    private string? city;

    [ObservableProperty]
    private string? biletType;

    [ObservableProperty]
    private string? price;

    [ObservableProperty]
    private string? points;

    [ObservableProperty]
    private bool hasActiveTickets = true;

    [RelayCommand]
    public async Task LoadActiveTicketsAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            int userId = Preferences.Get("UserId", 0);
            var result = await _ticketService.GetActiveTicketsAsync(userId);

            ActiveTickets.Clear();

            if (result.IsSuccess && result.Data != null)
            {
                foreach (var ticket in result.Data)
                {
                    ActiveTickets.Add(ticket);
                }
                HasActiveTickets = true;
            }
            else
            {
                HasActiveTickets = false;
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ShowQrCodePopupAsync(Ticket ticket)
    {
        var userUuid = await UserIdentifierService.GetOrCreateUserUUIDAsync();
        // Użycie serwisu do wczytania lub wygenerowania kodu QR
        ImageSource? imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(userUuid, ticket.QrCode!);

        if (imageSource == null)
        {
            // Jeśli kod QR nie został znaleziony w pamięci, generujemy go ponownie
            await _qrCodeStorageService.GenerateAndSaveQrCodeImageAsync(userUuid, ticket.QrCode!);
            imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(userUuid, ticket.QrCode!);
        }

        if (imageSource == null)
        {
            await Shell.Current.DisplayAlert("Błąd", "Nie udało się znaleźć lub wygenerować kodu QR.", "OK");
            return;
        }

        DateTime activationTime = Preferences.Get($"ActivationTime_{ticket.QrCode}", DateTime.MinValue);
        int? remainingTime = null;
        if (activationTime != DateTime.MinValue)
        {
            var elapsedTime = DateTime.UtcNow - activationTime;
            remainingTime = Math.Max(0, (int)(5 * 60 - elapsedTime.TotalSeconds));
        }

        QrCodePopup qrCodePopup = new(imageSource, _qrCodeService, userUuid, ticket.QrCode!, remainingTime);
        await Shell.Current.ShowPopupAsync(qrCodePopup);
    }
}