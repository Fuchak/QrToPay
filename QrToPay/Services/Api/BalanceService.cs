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

    public async Task<ServiceResult<decimal>> LoadUserDataAsync()
    {
        try
        {
            int userId = Preferences.Get("UserId", 0);
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            BalanceResponse? response = await client.GetFromJsonAsync<BalanceResponse>($"/api/UserBalance/{userId}/balance");

            if (response != null)
            {
                BalanceUpdated?.Invoke(this, response.AccountBalance ?? 0);
                return ServiceResult<decimal>.Success(response.AccountBalance ?? 0);
            }
            else
            {
                return ServiceResult<decimal>.Failure("Błąd: Nie udało się pobrać salda.");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<decimal>.Failure(HttpError.HandleError(ex));
        }
    }
}