using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QRCoder;
using CommunityToolkit.Maui.Views;
using QrToPay.Models.Common;
using QrToPay.Services;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class ActiveBiletsViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly QrCodeStorageService _qrCodeStorageService;

    public ActiveBiletsViewModel(IHttpClientFactory httpClientFactory, QrCodeStorageService qrCodeStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _qrCodeStorageService = qrCodeStorageService;
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
    private bool hasActiveTickets;

    [RelayCommand]
    public async Task LoadActiveTicketsAsync()
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            int userId = Preferences.Get("UserId", 0);
            HttpResponseMessage response = await client.GetAsync($"/api/Tickets/active?userId={userId}");
            response.EnsureSuccessStatusCode();

            List<Ticket>? tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
            ActiveTickets.Clear();
            if (tickets != null)
            {
                foreach (var ticket in tickets)
                {
                    ActiveTickets.Add(ticket);
                }
            }

            HasActiveTickets = ActiveTickets.Count > 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to load active tickets: {ex.Message}");
            HasActiveTickets = false;
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

        QrCodePopup qrCodePopup = new(imageSource);
        await Shell.Current.ShowPopupAsync(qrCodePopup);
    }
}