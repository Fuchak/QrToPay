using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Xml.Linq;
using System.Text.Json;
using QrToPay.Models.Responses;
using System.Net.Http.Json;
using System.Net.Http;
using System.Globalization;

namespace QrToPay.ViewModels;
public partial class TopUpAccountViewModel(IHttpClientFactory httpClientFactory) : ViewModelBase
{
    [ObservableProperty]
    private string? amount;

    [RelayCommand]
    private async Task NavigateToTopUp()
    {
        string amountWithDot = Amount?.Replace(',', '.');

        if (decimal.TryParse(amountWithDot, NumberStyles.Number, CultureInfo.InvariantCulture, out var topUpAmount) && topUpAmount > 0)
        {
            var userId = Preferences.Get("UserId", 0);
            var topUpRequest = new TopUpResponse
            {
                UserID = userId,
                Amount = topUpAmount
            };

            var client = httpClientFactory.CreateClient("ApiHttpClient");
            Debug.WriteLine($"Sending top-up request: {JsonSerializer.Serialize(topUpRequest)}");
            var response = await client.PostAsJsonAsync("/userdata/topup", topUpRequest);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Response data: {responseData}");

                using JsonDocument doc = JsonDocument.Parse(responseData);
                if (doc.RootElement.TryGetProperty("accountBalance", out JsonElement accountBalanceElement))
                {
                    decimal accountBalance = accountBalanceElement.GetDecimal();
                    Debug.WriteLine($"Top-up successful, new balance: {accountBalance}");
                    await Shell.Current.DisplayAlert("Potwierdzenie", $"Konto zostało doładowane. Nowe saldo: {accountBalance:C}", "OK");

                    Amount = string.Empty;
                }
                else
                {
                    Debug.WriteLine("Failed to retrieve account balance from response.");
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się doładować konta. Spróbuj ponownie.", "OK");
                }
            }
            else
            {
                Debug.WriteLine($"Top-up failed: {response.ReasonPhrase}");
                await Shell.Current.DisplayAlert("Błąd", "Nie udało się doładować konta. Spróbuj ponownie.", "OK");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Błąd", "Wprowadź poprawną kwotę doładowania.", "OK");
        }
    }
}