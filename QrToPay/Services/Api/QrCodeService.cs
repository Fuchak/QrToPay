using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;

namespace QrToPay.Services.Api;
public class QrCodeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public QrCodeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task ActivateQrCodeAsync(string token)
    {

        var requestModel = new
        {
            Token = token,
            //UserID = userId
        };

        var jwtToken = await SecureStorage.GetAsync("AuthToken");

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        await client.PostAsJsonAsync("api/Tickets/activate", requestModel);

    }

    public async Task DeactivateQrCodeAsync(string token)
    {//Todo zrobić model do tego xd
        var requestModel = new
        {
            Token = token,
            //UserID = userId
        };

        var jwtToken = await SecureStorage.GetAsync("AuthToken");

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        await client.PostAsJsonAsync("api/Tickets/deactivate", requestModel);
    }
}