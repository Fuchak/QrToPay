﻿using QrToPay.Models.Common;
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

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBalanceLoading;

    [RelayCommand]
    public async Task LoadUserDataAsync()
    {
        if(IsBusy) return;

        try
        {
            IsBusy = true;
            IsBalanceLoading = true;

            (decimal? balance, string? error) = await _balanceService.LoadUserDataAsync();
            if (balance.HasValue)
            {
                AccountBalance = balance.Value;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = error;
            }
        }
        finally
        {
            IsBusy = false;
            IsBalanceLoading = false;
        }
    }

    [RelayCommand]
    private Task NavigateToSkiResortPage() => NavigateAsync(nameof(SkiResortCityPage));

    [RelayCommand]
    private Task NavigateToFunFairPage() => NavigateAsync(nameof(FunFairCityPage));

    [RelayCommand]
    private Task NavigateToScanPage() => NavigateAsync(nameof(ScanQrCodePage));
}