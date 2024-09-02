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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly QrCodeStorageService _qrCodeStorageService;
    private readonly QrCodeService _qrCodeService;

    public ActiveBiletsViewModel(IHttpClientFactory httpClientFactory, QrCodeStorageService qrCodeStorageService, QrCodeService qrCodeService)
    {
        _httpClientFactory = httpClientFactory;
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

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            int userId = Preferences.Get("UserId", 0);
            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/active?userId={userId}");
            response.EnsureSuccessStatusCode();

            List<Ticket>? tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
            ActiveTickets.Clear();

            if (tickets != null && tickets.Count > 0)
            {
                foreach (var ticket in tickets)
                {
                    ActiveTickets.Add(ticket);
                }
                HasActiveTickets = true;
                ErrorMessage = null;
            }
            else
            {
                HasActiveTickets = false;
                ErrorMessage = null;
            }
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
    private async Task ShowQrCodePopupAsync(Ticket ticket)
    {
        // Użycie serwisu do wczytania lub wygenerowania kodu QR
        ImageSource? imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(ticket.UserId, ticket.QrCode!);

        if (imageSource == null)
        {
            // Jeśli kod QR nie został znaleziony w pamięci, generujemy go ponownie
            await _qrCodeStorageService.GenerateAndSaveQrCodeImageAsync(ticket.UserId, ticket.QrCode!);
            imageSource = await _qrCodeStorageService.LoadQrCodeImageAsync(ticket.UserId, ticket.QrCode!);
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

        QrCodePopup qrCodePopup = new(imageSource, _qrCodeService, ticket.UserId, ticket.QrCode!, remainingTime);
        await Shell.Current.ShowPopupAsync(qrCodePopup);
    }
}