using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using QRCoder;
using CommunityToolkit.Maui.Views;
using QrToPay.Models.Common;

namespace QrToPay.ViewModels.FlyoutMenu;

public partial class ActiveBiletsViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ActiveBiletsViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
    private void ShowQrCodePopup(Ticket ticket)
    {

        ImageSource imageSource = GenerateQrCodeImage(ticket.UserId, ticket.QrCode ?? string.Empty);
        QrCodePopup qrCodePopup = new (imageSource);

        Shell.Current.ShowPopup(qrCodePopup);
    }

    private ImageSource GenerateQrCodeImage(int userId, string qrCodePath)
    {
        QRCodeGenerator qRCodeGenerator = new();
        QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode($"UserId:{userId},QrCode:{qrCodePath}", QRCodeGenerator.ECCLevel.Q);
        BitmapByteQRCode bitmapByteQRCode = new (qRCodeData);
        byte[] bytes = bitmapByteQRCode.GetGraphic(20);
        
        return ImageSource.FromStream(() =>
        {
            MemoryStream memoryStream = new(bytes);
            return memoryStream;
        });
    }
}