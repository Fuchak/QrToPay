using QRCoder;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using BarcodeScanning;
using System.Globalization;

namespace QrToPay.Services;
public class QrCodeService(IHttpClientFactory httpClientFactory)
{
    public async Task<string> GenerateAndUpdateTicketAsync(UpdateTicketRequest updateRequest)
    {
        var client = httpClientFactory.CreateClient("ApiHttpClient");

        var response = await client.PostAsJsonAsync("/tickets/generateAndUpdate", updateRequest);

        if (!response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"Nieudane generowanie lub aktualizacja biletu: {response.ReasonPhrase}");
            return string.Empty;
        }

        var result = await response.Content.ReadFromJsonAsync<UpdateTicketResponse>();
        return result?.QrCode ?? string.Empty;
    }
}