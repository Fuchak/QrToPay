using BarcodeScanning;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;

namespace QrToPay.ViewModels;

public partial class ScanQrCodeViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private string? detectedCode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private Attraction? _currentAttraction;

    [ObservableProperty]
    private bool pauseScanning = false;


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
            }
            
        }
    }
    private async Task FetchAttractionData(string qrCode)
    {
        try
        {
            var client = httpClientFactory.CreateClient("ApiHttpClient");
            var response = await client.GetAsync($"/scan/{qrCode}");

            if (response.IsSuccessStatusCode)
            {
                var attraction = await response.Content.ReadFromJsonAsync<Attraction>();
                if (attraction != null)
                {
                    CurrentAttraction = attraction;
                    PurchaseCommand.NotifyCanExecuteChanged();
                }
            }
            else
            {
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Debug.WriteLine(errorContent?.Message ?? "Błąd podczas skanowania kodu QR.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching attraction data: {ex.Message}");
        }
    }

    public bool IsAttractionValid =>
            !string.IsNullOrEmpty(CurrentAttraction?.Type) &&
            !string.IsNullOrEmpty(CurrentAttraction?.AttractionName) &&
            CurrentAttraction?.Price != null;


    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task Purchase()
    {
        try
        {
            var client = httpClientFactory.CreateClient("ApiHttpClient");
            var userId = Preferences.Get("UserId", 0);

            var purchaseRequest = new PurchaseRequest
            {
                UserId = userId,
                Type = CurrentAttraction?.Type,
                AttractionName = CurrentAttraction?.AttractionName,
                Price = CurrentAttraction.Price
            };

            var response = await client.PostAsJsonAsync("/scan/purchase", purchaseRequest);

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
                var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                Debug.WriteLine(errorContent?.Message ?? "Błąd podczas zakupu.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during purchase: {ex.Message}");
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

    public async Task<bool> CheckPermissions()
    {
        PermissionStatus cameraStatus = await CheckPermissions<Permissions.Camera>();

        return IsGranted(cameraStatus);
    }
    private async Task<PermissionStatus> CheckPermissions<TPermission>() where TPermission : Permissions.BasePermission, new()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<TPermission>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<TPermission>();
        }

        return status;
    }

    private static bool IsGranted(PermissionStatus status)
    {
        return status == PermissionStatus.Granted || status == PermissionStatus.Limited; ;
    }
}