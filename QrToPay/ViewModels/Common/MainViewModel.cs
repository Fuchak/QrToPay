using QrToPay.Models.Common;

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

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBalanceLoaded;

    [RelayCommand]
    public async Task LoadUserDataAsync()
    {
        IsLoading = true;
        try
        {
            (decimal? balance, string? error) = await _balanceService.LoadUserDataAsync();
            if (balance.HasValue)
            {
                AccountBalance = balance.Value;
                IsBalanceLoaded = true;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = error;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    // [RelayCommand]
    // private Task NavigateToSkiPage() => NavigateAsync(nameof(SkiPage));

    // [RelayCommand]
    // private Task NavigateToFunfair() => NavigateAsync(nameof(FunFairPage));

    [RelayCommand]
    public async Task NavigateToCityPageAsync(string attractionType)
    {
        _appState.AttractionType = attractionType;
        await Shell.Current.GoToAsync(nameof(CityPage));
    }

    [RelayCommand]
    private Task NavigateToScanPage() => NavigateAsync(nameof(ScanQrCodePage));
}