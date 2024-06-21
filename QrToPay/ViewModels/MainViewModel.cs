using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace QrToPay.ViewModels
{
    public partial class MainViewModel(BalanceService balanceService, AppState appState) : ViewModelBase
    {

        [ObservableProperty]
        private int userID;

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
                (decimal? balance, string? error) = await balanceService.LoadUserDataAsync();
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
            appState.AttractionType = attractionType;
            await Shell.Current.GoToAsync(nameof(CityPage));
        }

        [RelayCommand]
        private Task NavigateToScanPage() => NavigateAsync(nameof(ScanQrCodePage));
    }
}