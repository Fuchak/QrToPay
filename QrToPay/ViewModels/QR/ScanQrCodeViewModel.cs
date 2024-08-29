using BarcodeScanning;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;

namespace QrToPay.ViewModels.QR;
public partial class ScanQrCodeViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ScanQrCodeViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private string? detectedCode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private Purchase? currentAttraction;

    [ObservableProperty]
    private bool pauseScanning = false;

    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string? errorMessage;

    private const int PauseDuration = 4000; // Pauza w milisekundach (4 sekundy) by nie zawalać api zbyt dużą ilością zapytań

    [RelayCommand]
    private async Task OnDetectionFinished(BarcodeResult[] results)
    {
        if (results.Length > 0 && !PauseScanning)
        {
            // Obsługa wykrytego kodu kreskowego
            DetectedCode = results[0].DisplayValue;
            await FetchAttractionData(DetectedCode);
            if (IsAttractionValid)
            {
                PauseScanning = true;
            }
            else
            { 
                CurrentAttraction = null;
                PauseScanning = true;
                await Task.Delay(PauseDuration); // Zatrzymanie skanowania na kilka sekund
                PauseScanning = false; // Wznowienie skanowania
            }
            
        }
    }
    private async Task FetchAttractionData(string qrCode)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Scan/{qrCode}");

            if (response.IsSuccessStatusCode)
            {
                Purchase? attraction = await response.Content.ReadFromJsonAsync<Purchase>();
                if (attraction != null)
                {
                    CurrentAttraction = attraction;
                    PurchaseCommand.NotifyCanExecuteChanged();
                }
            }
            else
            {
                ApiResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                ErrorMessage = $"Błąd: {errorResponse?.Message}";
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie.";
        }
    }

    public bool IsAttractionValid =>
            !string.IsNullOrEmpty(CurrentAttraction?.ServiceName) &&
            CurrentAttraction?.ServiceId != null &&
            !string.IsNullOrEmpty(CurrentAttraction?.AttractionName) &&
            CurrentAttraction?.Price != null;


    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task Purchase()
    {
        try
        {
            IsBusy = true;

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            int userId = Preferences.Get("UserId", 0);

            if (CurrentAttraction != null)
            {
                PurchaseRequest purchaseRequest = new()
                {
                    UserId = userId,
                    Purchase = new Purchase
                    {
                        ServiceName = CurrentAttraction.ServiceName,
                        ServiceId = CurrentAttraction.ServiceId,
                        AttractionName = CurrentAttraction.AttractionName,
                        Price = CurrentAttraction!.Price
                    }
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Scan/purchase", purchaseRequest);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Zakup", "Zakup zakończony pomyślnie!", "OK");

                    DetectedCode = null;
                    CurrentAttraction = null;
                    PauseScanning = false;

                    PurchaseCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    ApiResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    ErrorMessage = $"Błąd: {errorResponse?.Message}";
                }
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Wystąpił nieoczekiwany błąd. Spróbuj ponownie.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        // Resetowanie danych po anulowaniu
        DetectedCode = null;
        CurrentAttraction = null;
        PauseScanning = false; // Ponownie włącz skanowanie po anulowaniu

        // Zaktualizowanie stanu komendy
        PurchaseCommand.NotifyCanExecuteChanged();
    }

    private bool CanPurchase()
    {
        return IsAttractionValid;
    }
}