using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QrToPay.Models;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using QRCoder;
using System.Drawing;
using System.IO;
using CommunityToolkit.Maui.Views;

namespace QrToPay.ViewModels;

public partial class ActiveBiletsViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
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
            var client = httpClientFactory.CreateClient("ApiHttpClient");
            int userId = Preferences.Get("UserId", 0);
            var response = await client.GetAsync($"/tickets/active?userId={userId}");
            response.EnsureSuccessStatusCode();

            var ticketsFromApi = await response.Content.ReadFromJsonAsync<List<Ticket>>();
            ActiveTickets.Clear();
            if (ticketsFromApi != null)
            {
                foreach (var ticket in ticketsFromApi)
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

        var qrCodeImage = GenerateQrCodeImage(ticket.UserId, ticket.QrCode ?? string.Empty);
        var popup = new QrCodePopup(qrCodeImage);

        Shell.Current.ShowPopup(popup);
    }

    private ImageSource GenerateQrCodeImage(int userId, string qrCodePath)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode($"UserId:{userId},QrCode:{qrCodePath}", QRCodeGenerator.ECCLevel.Q);
        var qrCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeImageBytes = qrCode.GetGraphic(20);

        var stream = new MemoryStream(qrCodeImageBytes);
        return ImageSource.FromStream(() =>
        {
            var newStream = new MemoryStream(qrCodeImageBytes);
            return newStream;
        });
    }
}
