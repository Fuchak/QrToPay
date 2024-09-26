using CommunityToolkit.Maui.Views;
using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.QR;
public partial class QrCodePopupViewModel : ViewModelBase
{
    [ObservableProperty]
    private ImageSource qrCodeImage;

    [ObservableProperty]
    private int remainingTime;

    [ObservableProperty]
    private string remainingTimeDisplay = "Kod nie jest aktywny.";

    [ObservableProperty]
    private bool isActive;

    private readonly QrCodeService _qrCodeService;
    private readonly CacheService _cacheService;
    private readonly string _token;

    public QrCodePopupViewModel(ImageSource qrCodeImage, QrCodeService qrCodeService, CacheService cacheService, string token, int? remainingTime = null)
    {
        QrCodeImage = qrCodeImage;
        _qrCodeService = qrCodeService;
        _cacheService = cacheService;
        _token = token;

        InitializeRemainingTime(remainingTime);

        UpdateRemainingTimeDisplay();
        StartTimer();
    }

    private async void InitializeRemainingTime(int? remainingTime)
    {
        if (remainingTime.HasValue)
        {
            RemainingTime = remainingTime.Value;
            IsActive = true;
        }
        else
        {
            var activationTime = await _cacheService.LoadValueFromCacheAsync<DateTime>($"{AppDataConst.QrActivationTime}_{_token}");
            if (activationTime.HasValue)
            {
                SetRemainingTimeFromActivation(activationTime.Value);
            }
        }
    }

    [RelayCommand]
    public async Task ClosePopupAsync(Popup popup) => await popup.CloseAsync();


    [RelayCommand]
    public async Task ActivateQrCodeAsync()
    {
        var result = await _qrCodeService.ActivateQrCodeAsync(_token);

        if (result.IsSuccess)
        {
            SetActivationState(5 * 60, true);
            await _cacheService.SaveToCacheAsync($"{AppDataConst.QrActivationTime}_{_token}", DateTime.UtcNow);
            ErrorMessage = null; // Wyczyszczenie błędu po udanej aktywacji
        }
        else
        {
            ErrorMessage = result.ErrorMessage ?? "Nie udało się aktywować kodu QR.";
        }
    }

    [RelayCommand]
    public async Task DeactivateQrCodeAsync()
    {
        var result = await _qrCodeService.DeactivateQrCodeAsync(_token);

        if (result.IsSuccess)
        {
            SetActivationState(0, false);
            _cacheService.RemoveFromCache($"{AppDataConst.QrActivationTime}_{_token}");
            ErrorMessage = null;
        }
        else
        {
            ErrorMessage = result.ErrorMessage ?? "Nie udało się dezaktywować kodu QR.";
        }
    }

    private void SetActivationState(int time, bool isActive)
    {
        RemainingTime = time;
        IsActive = isActive;
        UpdateRemainingTimeDisplay();
        if (isActive) StartTimer();
    }

    private void StartTimer()
    {
        Shell.Current.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            if (--RemainingTime <= 0)
            {
                SetActivationState(0, false);
                _cacheService.RemoveFromCache($"{AppDataConst.QrActivationTime}_{_token}");
                return false;
            }

            UpdateRemainingTimeDisplay();
            return true;
        });
    }

    private void UpdateRemainingTimeDisplay()
    {
        RemainingTimeDisplay = IsActive
            ? $"Pozostały czas: {RemainingTime / 60:D2}:{RemainingTime % 60:D2}"
            : "Kod nie jest aktywny.";
    }

    private void SetRemainingTimeFromActivation(DateTime activationTime)
    {
        if (activationTime == DateTime.MinValue)
        {
            IsActive = false;
            RemainingTime = 0;
            return;
        }

        var elapsedTime = DateTime.UtcNow - activationTime;
        RemainingTime = Math.Max(0, (int)(5 * 60 - elapsedTime.TotalSeconds));
        IsActive = RemainingTime > 0;

        if (!IsActive)
        {
            _cacheService.RemoveFromCache($"{AppDataConst.QrActivationTime}_{_token}");
        }
    }
}