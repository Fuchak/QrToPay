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
        string? amountWithDot = Amount?.Replace(',', '.');

        if (IsBusy) return;

        if (decimal.TryParse(amountWithDot, NumberStyles.Number, CultureInfo.InvariantCulture, out var topUpAmount) && topUpAmount > 0)
        {
            string formattedAmount = topUpAmount.ToString("F2", CultureInfo.InvariantCulture);

            int userId = Preferences.Get("UserId", 0);

            TopUpRequest topUpRequest = new()
            {
                UserId = userId,
                Amount = decimal.Parse(formattedAmount, CultureInfo.InvariantCulture)
            };

            try
            {
                IsBusy = true;
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
            catch (Exception ex)
            {
                ErrorMessage = HttpError.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        else
        {
            ErrorMessage = "Wprowadź poprawną kwotę doładowania.";
        }
    }
}