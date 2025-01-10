using BarcodeScanning;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using System.Text.Json;

namespace QrToPay.ViewModels.QR;
public partial class ScanQrCodeViewModel : ViewModelBase
{
    private readonly ScanService _scanService;

    public ScanQrCodeViewModel(ScanService scanService)
    {
        _scanService = scanService;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private string? detectedCode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAttractionValid))]
    private Purchase? currentAttraction;

    [ObservableProperty]
    private bool pauseScanning = false;

    private const int PauseDuration = 2000; // Pauza w milisekundach (2 sekundy) by nie zawalać api zbyt dużą ilością zapytań

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
                Vibration.Vibrate(TimeSpan.FromSeconds(0.2));
            }
            else
            {
                CurrentAttraction = null;
                PauseScanning = true;
                await Task.Delay(PauseDuration);
                PauseScanning = false;
            }
        }
    }
    private async Task FetchAttractionData(string qrCode)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _scanService.FetchAttractionDataAsync(qrCode);

            if (result.IsSuccess && result.Data != null)
            {
                CurrentAttraction = result.Data;
                ErrorMessage = null;
                SuccessMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                CurrentAttraction = null;
            }
        }
        finally
        {
            IsBusy = false;
            PurchaseCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task Purchase()
    {
        if (IsBusy || CurrentAttraction == null) return;
        try
        {
            IsBusy = true;

            if (CurrentAttraction != null)
            {
                PurchaseRequest purchaseRequest = new()
                {
                    ServiceName = CurrentAttraction.ServiceName,
                    ServiceId = CurrentAttraction.ServiceId,
                    AttractionName = CurrentAttraction.AttractionName,
                    Price = CurrentAttraction.Price
                };
                string jsonToSend = JsonSerializer.Serialize(purchaseRequest);
                Debug.WriteLine(jsonToSend);

                var result = await _scanService.MakePurchaseAsync(purchaseRequest);

                if (result.IsSuccess)
                {
                    SuccessMessage = "Zakup zakończony pomyślnie!";

                    Cancel();
                }
                else
                {
                    ErrorMessage = result.ErrorMessage 
                        ?? "Zakup nie powiódł się.";
                }
            }
        }
        finally
        {
            IsBusy = false;
            PurchaseCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        DetectedCode = null;
        CurrentAttraction = null;
        PauseScanning = false;
        PurchaseCommand.NotifyCanExecuteChanged();
    }

    public bool IsAttractionValid =>
        !string.IsNullOrEmpty(CurrentAttraction?.ServiceName) &&
        CurrentAttraction?.ServiceId != null &&
        !string.IsNullOrEmpty(CurrentAttraction?.AttractionName) &&
        CurrentAttraction?.Price != null;

    private bool CanPurchase() => IsAttractionValid;
}