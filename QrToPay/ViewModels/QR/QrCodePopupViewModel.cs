using CommunityToolkit.Maui.Views;
using QrToPay.Services.Api;

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
    private readonly string _token;

    public QrCodePopupViewModel(ImageSource qrCodeImage, QrCodeService qrCodeService, string token, int? remainingTime = null)
    {
        QrCodeImage = qrCodeImage;
        _qrCodeService = qrCodeService;
        _token = token;

        InitializeRemainingTime(remainingTime);

        UpdateRemainingTimeDisplay();
        StartTimer();
    }

    private void InitializeRemainingTime(int? remainingTime)
    {
        if (remainingTime.HasValue)
        {
            RemainingTime = remainingTime.Value;
            IsActive = true;
        }
        else
        {
            DateTime activationTime = Preferences.Get($"ActivationTime_{_token}", DateTime.MinValue);
            SetRemainingTimeFromActivation(activationTime);
        }
    }

    [RelayCommand]
    public async Task ClosePopupAsync(Popup popup) => await popup.CloseAsync();


    [RelayCommand]
    public async Task ActivateQrCodeAsync()
    {
        await _qrCodeService.ActivateQrCodeAsync(_token);
        SetActivationState(5 * 60, true);
        Preferences.Set($"ActivationTime_{_token}", DateTime.UtcNow);
    }

    [RelayCommand]
    public async Task DeactivateQrCodeAsync()
    {
        await _qrCodeService.DeactivateQrCodeAsync(_token);
        SetActivationState(0, false);
        Preferences.Remove($"ActivationTime_{_token}");
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
                Preferences.Remove($"ActivationTime_{_token}");
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
            Preferences.Remove($"ActivationTime_{_token}");
        }
    }
}