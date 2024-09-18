using QrToPay.Services.Api;
using QrToPay.Services.Local;

namespace QrToPay.ViewModels.Common;

public partial class LoadingViewModel : ViewModelBase
{
    private readonly AuthService _authService;
    private readonly QrCodeStorageService _qrCodeStorageService;

    public LoadingViewModel(AuthService authService, QrCodeStorageService qrCodeStorageService)
    {
        _authService = authService;
        _qrCodeStorageService = qrCodeStorageService;
    }

    public async Task CheckAuthentication()
    {
        CleanOldQrCodes();

        bool isAuthenticated = await _authService.IsAuthenticatedAsync();

        if (isAuthenticated)
        {
            await NavigateAsync($"///{nameof(MainPage)}");
        }
        else
        {
            await NavigateAsync($"///{nameof(LoginPage)}");
        }
    }

    private void CleanOldQrCodes()
    {
        _qrCodeStorageService.CleanOldQrCodeFiles(TimeSpan.FromDays(3));
    }
}