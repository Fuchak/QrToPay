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

    [ObservableProperty]
    private string? errorMessage;

    [RelayCommand]
    private async Task NavigateToTopUp()
    {
        string? amountWithDot = Amount?.Replace(',', '.');

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
                HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
                Debug.WriteLine($"Sending top-up request: {JsonSerializer.Serialize(topUpRequest)}");
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/UserBalance/topUp", topUpRequest);

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
                        ErrorMessage = null;
                    }
                    else
                    {
                        ErrorMessage = "Nie udało się pobrać salda konta z odpowiedzi.";
                    }
                }
                else
                {
                    ErrorMessage = HttpError.HandleHttpError(new HttpRequestException(response.ReasonPhrase, null, response.StatusCode));
                }
            }
            catch (HttpRequestException httpEx)
            {
                ErrorMessage = HttpError.HandleHttpError(httpEx);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił nieoczekiwany błąd: {ex.Message}";
            }
        }
        else
        {
            ErrorMessage = "Wprowadź poprawną kwotę doładowania.";
        }
    }
}