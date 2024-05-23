using BarcodeScanning;
using System.Text.Json;
using System.Windows.Input;

namespace QrToPay.ViewModels;

public partial class ScanQrCodeViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private string? detectedCode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private Attraction? _currentAttraction;

    [ObservableProperty]
    private bool pauseScanning;

    public ICommand DetectionFinishedCommand { get; set; }

    public ScanQrCodeViewModel()
    {
        DetectionFinishedCommand = new Command<BarcodeResult[]>(OnDetectionFinished);
        PauseScanning = false;
    }

    private void OnDetectionFinished(BarcodeResult[] results)
    {
        if (results.Length > 0 && !PauseScanning)
        {
            // Obsługa wykrytego kodu kreskowego
            DetectedCode = results[0].DisplayValue;
            ParseQrCode(DetectedCode);
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
    private void ParseQrCode(string qrCode)
    {
        try
        {
            var attraction = JsonSerializer.Deserialize<Attraction>(qrCode);
            if (attraction != null)
            {
                CurrentAttraction = attraction;
                PurchaseCommand.NotifyCanExecuteChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing QR code: {ex.Message}");
        }
    }

    public bool IsAttractionValid =>
            !string.IsNullOrEmpty(CurrentAttraction?.Type) &&
            !string.IsNullOrEmpty(CurrentAttraction?.AttractionName) &&
            CurrentAttraction?.Price.HasValue == true;


    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task Purchase()
    {
        // Logika zakupu
        await Shell.Current.DisplayAlert("Zakup", "Zakup zakończony pomyślnie!", "OK");

        DetectedCode = null;
        CurrentAttraction = null;
        PauseScanning = false;

        PurchaseCommand.NotifyCanExecuteChanged();
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