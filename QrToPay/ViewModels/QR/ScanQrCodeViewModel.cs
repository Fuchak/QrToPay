using BarcodeScanning;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;

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
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            var result = await _scanService.FetchAttractionDataAsync(qrCode);

            if (result.IsSuccess && result.Data != null)
            {
                CurrentAttraction = result.Data;
                PurchaseCommand.NotifyCanExecuteChanged();
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
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

    [RelayCommand(CanExecute = nameof(CanPurchase))]
    private async Task Purchase()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

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

                var result = await _scanService.MakePurchaseAsync(purchaseRequest);

                if (result.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Zakup", "Zakup zakończony pomyślnie!", "OK");

                    DetectedCode = null;
                    CurrentAttraction = null;
                    PauseScanning = false;

                    PurchaseCommand.NotifyCanExecuteChanged();
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
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
    private void Cancel()
    {
        // Resetowanie danych po anulowaniu
        DetectedCode = null;
        CurrentAttraction = null;
        PauseScanning = false; // Ponownie włącz skanowanie po anulowaniu

        // Zaktualizowanie stanu komendy
        PurchaseCommand.NotifyCanExecuteChanged();
    }

    public bool IsAttractionValid =>
        !string.IsNullOrEmpty(CurrentAttraction?.ServiceName) &&
        CurrentAttraction?.ServiceId != null &&
        !string.IsNullOrEmpty(CurrentAttraction?.AttractionName) &&
        CurrentAttraction?.Price != null;

    private bool CanPurchase()
    {
        return IsAttractionValid;
    }
}