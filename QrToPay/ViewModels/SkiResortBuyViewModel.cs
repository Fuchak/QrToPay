
using Microsoft.Maui.Controls;

namespace QrToPay.ViewModels;

public partial class SkiResortBuyViewModel(QrCodeService qrCodeService) : ViewModelBase, IQueryAttributable
{
    private readonly QrCodeService _qrCodeService = qrCodeService;

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
    public string? title;

    public string PointsPrice => $"{Points} - {Price}";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ResortName = Uri.UnescapeDataString(query["ResortName"] as string ?? string.Empty);
        City = Uri.UnescapeDataString(query["City"] as string ?? string.Empty);
        BiletType = Uri.UnescapeDataString(query["BiletType"] as string ?? string.Empty);
        Points = Uri.UnescapeDataString(query["Points"] as string ?? string.Empty);
        Price = Uri.UnescapeDataString(query["Price"] as string ?? string.Empty);
        Title = Uri.UnescapeDataString(query["Title"] as string ?? string.Empty);

        OnPropertyChanged(nameof(PointsPrice));
    }

    [RelayCommand]
    private async Task GenerateQrCode()
    {
        //Dodane by pozbyć się błędów że niby są null a wiemy przecież że nie mogą być i nigdy nie będą
        if (string.IsNullOrEmpty(ResortName) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(BiletType) || string.IsNullOrEmpty(Price) || string.IsNullOrEmpty(Points))
        {
            await Shell.Current.DisplayAlert("Błąd", "Wszystkie pola muszą być wypełnione", "OK");
            return;
        }

        await Shell.Current.DisplayAlert("Potwierdzenie", "Bilet został zakupiony, kod qr wygenerowany", "OK");
        var qrCodePath = await _qrCodeService.GenerateQrCodeAsync(ResortName, City, BiletType, Price, Points);
        await Shell.Current.GoToAsync($"//MainPage/ActiveBiletsPage?QrCodePath={qrCodePath}");
    }
}