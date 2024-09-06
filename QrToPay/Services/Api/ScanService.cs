using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using System.Net.Http;
using System.Threading.Tasks;
using QrToPay.Models.Common;

namespace QrToPay.Services.Api;

public class ScanService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ScanService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ServiceResult<Purchase>> FetchAttractionDataAsync(string qrCode)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Scan/{qrCode}");

            if (response.IsSuccessStatusCode)
            {
                Purchase? attraction = await response.Content.ReadFromJsonAsync<Purchase>();
                if (attraction != null)
                {
                    return ServiceResult<Purchase>.Success(attraction);
                }
                return ServiceResult<Purchase>.Failure("Nie udało się odczytać danych o atrakcji.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                                ?? "Błąd pobierania danych";
                return ServiceResult<Purchase>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<Purchase>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<object>> MakePurchaseAsync(PurchaseRequest purchaseRequest)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Scan/purchase", purchaseRequest);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<object>.Success();
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                                    ?? "Błąd płatności";

                return ServiceResult<object>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<object>.Failure(HttpError.HandleError(ex));
        }
    }
}