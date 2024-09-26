using System.Collections.ObjectModel;
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
    private readonly CacheService _cacheService;

    public ActiveBiletsViewModel(TicketService ticketService, QrCodeStorageService qrCodeStorageService, QrCodeService qrCodeService, CacheService cacheService)
    {
        _ticketService = ticketService;
        _qrCodeStorageService = qrCodeStorageService;
        _qrCodeService = qrCodeService;
        _cacheService = cacheService;
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

            var cachedActiveTickets = await _cacheService.LoadFromCacheAsync<IEnumerable<Ticket>>(AppDataConst.ActiveTickets);
            if (cachedActiveTickets != null)
            {
                CacheService.UpdateCollection(ActiveTickets, cachedActiveTickets);
            }

            var result = await _ticketService.GetActiveTicketsAsync();
            if (result.IsSuccess && result.Data != null)
            {
                var newActiveTickets = result.Data;

                if (!CacheService.AreDataEqual(ActiveTickets, newActiveTickets))
                {
                    await _cacheService.SaveToCacheAsync(AppDataConst.ActiveTickets, newActiveTickets);
                    CacheService.UpdateCollection(ActiveTickets, newActiveTickets);
                }

                HasActiveTickets = true;
            }
            else
            {
                if (!ActiveTickets.Any())
                {
                    HasActiveTickets = false;
                    ErrorMessage = result.ErrorMessage;
                }
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
        // Użycie serwisu do wczytania lub wygenerowania kodu QR
        ImageSource? imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(ticket.QrCode!);

        if (imageSource == null)
        {
            // Jeśli kod QR nie został znaleziony w pamięci, generujemy go ponownie
            await _qrCodeStorageService.GenerateAndSaveQrCodeImageAsync(ticket.QrCode!);
            imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(ticket.QrCode!);
        }

        if (imageSource == null)
        {
            await Shell.Current.DisplayAlert("Błąd", "Nie udało się znaleźć lub wygenerować kodu QR.", "OK");
            return;
        }

        var cacheKey = $"{AppDataConst.QrActivationTime}_{ticket.QrCode}";
        var activationTime = await _cacheService.LoadValueFromCacheAsync<DateTime>(cacheKey);
        int? remainingTime = null;
        if (activationTime.HasValue && activationTime != DateTime.MinValue)
        {
            var elapsedTime = DateTime.UtcNow - activationTime;
            if (elapsedTime.HasValue)
            {
                remainingTime = Math.Max(0, (int)(5 * 60 - elapsedTime.Value.TotalSeconds));
            }
        }

        QrCodePopup qrCodePopup = new(imageSource, _qrCodeService, _cacheService, ticket.QrCode!, remainingTime);
        await Shell.Current.ShowPopupAsync(qrCodePopup);
    }
}