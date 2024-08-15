using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using QrToPay.Models.Responses;

namespace QrToPay.Services;
public class BalanceService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BalanceService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private int userId;

    public event EventHandler<decimal>? BalanceUpdated;

    public async Task<(decimal? AccountBalance, string? ErrorMessage)> LoadUserDataAsync()
    {
        try
        {
            userId = Preferences.Get("UserId", 0);
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            UserResponse? userResponse = await client.GetFromJsonAsync<UserResponse>($"/api/UserBalance/{userId}/balance");

            if (userResponse != null)
            {
                BalanceUpdated?.Invoke(this, userResponse.AccountBalance ?? 0);
                return (userResponse.AccountBalance, null);
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
            return (null, "Wystąpił nieoczekiwany błąd.");
        }
    }
}