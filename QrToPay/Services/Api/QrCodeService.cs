using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Net.Http;
using System.Text.Json;

namespace QrToPay.Services.Api;
public class QrCodeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public QrCodeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GenerateAndUpdateTicketAsync(UpdateTicketRequest updateRequest)
    {
        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Tickets/generateAndUpdate", updateRequest);

        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        UpdateTicketResponse? result = await response.Content.ReadFromJsonAsync<UpdateTicketResponse>();
        return result?.QrCode ?? string.Empty;
    }

    public async Task ActivateQrCodeAsync(string token, int userId)
    {

        var requestModel = new
        {
            Token = token,
            UserID = userId
        };

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        await client.PostAsJsonAsync("api/Tickets/activate", requestModel);

    }

    public async Task DeactivateQrCodeAsync(string token, int userId)
    {//Todo zrobić model do tego xd
        var requestModel = new
        {
            Token = token,
            UserID = userId
        };

        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        await client.PostAsJsonAsync("api/Tickets/deactivate", requestModel);
    }
}