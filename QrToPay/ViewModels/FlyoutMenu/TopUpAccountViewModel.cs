using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;
using System.Globalization;
using QrToPay.Models.Requests;
using QrToPay.Services.Api;

namespace QrToPay.ViewModels.FlyoutMenu;
public partial class TopUpAccountViewModel : ViewModelBase
{
    private readonly BalanceService _balanceService;

    public TopUpAccountViewModel(BalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [ObservableProperty]
    private string? amount;

    [RelayCommand]
    private async Task NavigateToTopUp()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            string? amountWithDot = Amount?.Replace(',', '.');

            if (decimal.TryParse(amountWithDot, NumberStyles.Number, CultureInfo.InvariantCulture, out var topUpAmount) && topUpAmount > 0)
            {
            string formattedAmount = topUpAmount.ToString("F2", CultureInfo.InvariantCulture);

                TopUpRequest topUpRequest = new()
                {
                    Amount = decimal.Parse(formattedAmount, CultureInfo.InvariantCulture)
                };

                var result = await _balanceService.TopUpAccountAsync(topUpRequest);

                if (result.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("Potwierdzenie", $"Konto zostało doładowane. Nowe saldo: {result.Data:C}", "OK");
                    Amount = string.Empty;
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
            }
            else
            {
                ErrorMessage = "Wprowadź poprawną kwotę doładowania.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}