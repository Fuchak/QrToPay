using CommunityToolkit.Maui.Views;

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
    private readonly int _userId;
    private readonly string _token;

    public QrCodePopupViewModel(ImageSource qrCodeImage, QrCodeService qrCodeService, int userId, string token, int? remainingTime = null)
    {
        QrCodeImage = qrCodeImage;
        _qrCodeService = qrCodeService;
        _userId = userId;
        _token = token;

        if (remainingTime.HasValue)
        {
            IsActive = true;
            RemainingTime = remainingTime.Value;
        }
        else
        {
            // Sprawdzenie, czy czas aktywacji jest już zapisany
            DateTime activationTime = Preferences.Get($"ActivationTime_{_token}", DateTime.MinValue);
            if (activationTime != DateTime.MinValue)
            {
                SetRemainingTime(activationTime);
            }
            IsActive = false;
        }

        UpdateRemainingTimeDisplay();
        StartTimer();
    }

    [RelayCommand]
    public async Task ClosePopupAsync(Popup popup)
    {
        await popup.CloseAsync();
    }

    [RelayCommand]
    public async Task ActivateQrCodeAsync()
    {
        await _qrCodeService.ActivateQrCodeAsync(_token, _userId);

        RemainingTime = 5 * 60;
        IsActive = true;
        UpdateRemainingTimeDisplay();
        StartTimer();

        // Zapisz czas aktywacji w Preferencjach
        Preferences.Set($"ActivationTime_{_token}", DateTime.UtcNow);
    }

    [RelayCommand]
    public async Task DeactivateQrCodeAsync()
    {
        await _qrCodeService.DeactivateQrCodeAsync(_token, _userId);

        RemainingTime = 0;
        RemainingTimeDisplay = "Kod nie jest aktywny.";
        IsActive = false;

        Preferences.Remove($"ActivationTime_{_token}");
    }

    private void StartTimer()
    {
        Shell.Current.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            RemainingTime--;

            UpdateRemainingTimeDisplay();

            if (RemainingTime <= 0)
            {
                Preferences.Remove($"ActivationTime_{_token}");
                return false;
            }
            return true; 
        });
    }

    private async void UpdateRemainingTimeDisplay()
    {
        if (RemainingTime > 0)
        {
            RemainingTimeDisplay = $"Pozostały czas: {RemainingTime / 60:D2}:{RemainingTime % 60:D2}";
        }
        else
        {
            await DeactivateQrCodeAsync();
        }
    }

    private void SetRemainingTime(DateTime activationTime)
    {
        var elapsedTime = DateTime.UtcNow - activationTime;
        RemainingTime = Math.Max(0, (int)(5 * 60 - elapsedTime.TotalSeconds));

        if (RemainingTime <= 0)
        {
            Preferences.Remove($"ActivationTime_{_token}");
        }
    }
}