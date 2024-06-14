using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace QrToPay.Services
{
    public class BalanceService(IHttpClientFactory httpClientFactory)
    {
        private int userID;

        public event EventHandler<decimal>? BalanceUpdated;

        public async Task<(decimal? AccountBalance, string? ErrorMessage)> LoadUserDataAsync()
        {
            try
            {
                userID = Preferences.Get("UserId", 0);
                var client = httpClientFactory.CreateClient("ApiHttpClient");
                var user = await client.GetFromJsonAsync<User>($"/userdata/{userID}/balance");

                if (user != null)
                {
                    user.AccountBalance ??= 0;
                    BalanceUpdated?.Invoke(this, user.AccountBalance.Value);
                    return (user.AccountBalance, null);
                }
                else
                {
                    return (null, "Błąd: Nie udało się pobrać salda.");
                }
            }
            catch (HttpRequestException)
            {
                return (null, "Brak połączenia z internetem.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex}");
                return (null, "Wystąpił nieoczekiwany błąd.");
            }
        }
    }
}
