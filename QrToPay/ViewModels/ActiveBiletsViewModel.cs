
using System.Text.Json;

namespace QrToPay.ViewModels;

public partial class ActiveBiletsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string qrCodePath;

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

    public ActiveBiletsViewModel()
    {
        QrCodePath = Preferences.Get("QrCodePath", string.Empty);
        IsQrCodeVisible = false;

        if (!string.IsNullOrEmpty(QrCodePath))
        {
            var json = File.ReadAllText(Path.ChangeExtension(QrCodePath, ".json"));
            var document = JsonDocument.Parse(json);

            ResortName = document.RootElement.GetProperty(nameof(ResortName)).GetString();
            City = document.RootElement.GetProperty(nameof(City)).GetString();
            BiletType = document.RootElement.GetProperty(nameof(BiletType)).GetString();
            Price = document.RootElement.GetProperty(nameof(Price)).GetString();
            Points = document.RootElement.GetProperty(nameof(Points)).GetString();

            HasActiveTickets = true;
        }
        else
        {
            HasActiveTickets = false;
        }
    }

    [RelayCommand]
    private void ToggleQrCodeVisibility()
    {
        IsQrCodeVisible = !IsQrCodeVisible;
    }
}
