using QrToPay.Models.Common;
using QrToPay.Models.Enums;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.Common;

public partial class MainViewModel : ViewModelBase
{
    private readonly BalanceService _balanceService;
    private readonly AppState _appState;

    public MainViewModel(BalanceService balanceService, AppState appState)
    {
        _balanceService = balanceService;
        _appState = appState;
    }

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private decimal? accountBalance;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? phoneNumber;

    [RelayCommand]
    public async Task LoadUserDataAsync()
    {
        if(IsBusy || IsLoading) return;

        try
        {
            IsLoading = true;

            int userId = Preferences.Get("UserId", 0);

            var result = await _balanceService.LoadUserDataAsync(userId);
            if (result.IsSuccess)
            {
                AccountBalance = result.Data;
                ErrorMessage = null;
            }
            else
            {
                AccountBalance = 0.00m;
                ErrorMessage = result.ErrorMessage;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task NavigateToSkiResortPage() => NavigateAsync(nameof(SkiResortCityPage));

    [RelayCommand]
    private Task NavigateToFunFairPage() => NavigateAsync(nameof(FunFairCityPage));

    [RelayCommand]
    private Task NavigateToScanPage() => NavigateAsync(nameof(ScanQrCodePage));
}