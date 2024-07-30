using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;
using System.Globalization;
using QrToPay.Models.Requests;

namespace QrToPay.ViewModels.FlyoutMenu;
public partial class TopUpAccountViewModel : ViewModelBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TopUpAccountViewModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    private string? amount;

    //TODO widzę coś nie tak z tym topurequest a respoonse
    [RelayCommand]
    private async Task NavigateToTopUp()
    {
        string? amountWithDot = Amount?.Replace(',', '.');

        if (decimal.TryParse(amountWithDot, NumberStyles.Number, CultureInfo.InvariantCulture, out var topUpAmount) && topUpAmount > 0)
        {
            int  userId = Preferences.Get("UserId", 0);
            TopUpRequest topUpRequest = new()
            {
                UserId = userId,
                Amount = topUpAmount
            };

            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            Debug.WriteLine($"Sending top-up request: {JsonSerializer.Serialize(topUpRequest)}");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/UserData/topUp", topUpRequest);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
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