using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;
public class BalanceService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BalanceService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public event EventHandler<decimal>? BalanceUpdated;

    public async Task<(decimal? AccountBalance, string? ErrorMessage)> LoadUserDataAsync()
    {
        int userId;
        try
        {
            userId = Preferences.Get("UserId", 0);
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            BalanceResponse? response = await client.GetFromJsonAsync<BalanceResponse>($"/api/UserBalance/{userId}/balance");

            if (response != null)
            {
                BalanceUpdated?.Invoke(this, response.AccountBalance ?? 0);
                return (response.AccountBalance, null);
            }
            else
            {
                return (null, "Błąd: Nie udało się pobrać salda.");
            }
        }
        catch (HttpRequestException httpEx)
        {
            return (null, HttpError.HandleHttpError(httpEx));
        }
        catch (Exception)
        {
            return (null, HttpError.HandleGeneralError());
        }
    }
}