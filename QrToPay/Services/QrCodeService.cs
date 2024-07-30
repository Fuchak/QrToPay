using System.Diagnostics;
using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;

namespace QrToPay.Services;
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
            Debug.WriteLine($"Nieudane generowanie lub aktualizacja biletu: {response.ReasonPhrase}");
            return string.Empty;
        }

        UpdateTicketResponse? result = await response.Content.ReadFromJsonAsync<UpdateTicketResponse>();
        return result?.QrCode ?? string.Empty;
    }
}