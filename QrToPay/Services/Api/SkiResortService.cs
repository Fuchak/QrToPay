using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;

public class SkiResortService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SkiResortService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ServiceResult<List<SkiResortPriceResponse>>> GetSkiResortPricesAsync(int skiResortId)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/prices?skiResortId={skiResortId}");

            if (response.IsSuccessStatusCode)
            {
                List<SkiResortPriceResponse>? skiResortPrices = await response.Content.ReadFromJsonAsync<List<SkiResortPriceResponse>>();
                if (skiResortPrices != null)
                {
                    return ServiceResult<List<SkiResortPriceResponse>>.Success(skiResortPrices);
                }
                return ServiceResult<List<SkiResortPriceResponse>>.Failure("Nie udało się pobrać cen biletów narciarskich.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas pobierania cen biletów narciarskich.";
                return ServiceResult<List<SkiResortPriceResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<SkiResortPriceResponse>>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<List<SkiResortData>>> GetSkiResortsAsync(string cityName)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/resorts?cityName={cityName}");

            if (response.IsSuccessStatusCode)
            {
                List<SkiResortData>? skiResorts = await response.Content.ReadFromJsonAsync<List<SkiResortData>>();
                if (skiResorts != null)
                {
                    return ServiceResult<List<SkiResortData>>.Success(skiResorts);
                }
                return ServiceResult<List<SkiResortData>>.Failure("Nie udało się pobrać danych ze stoku narciarskiego.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas pobierania danych ze stoku narciarskiego.";
                return ServiceResult<List<SkiResortData>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<SkiResortData>>.Failure(HttpError.HandleError(ex));
        }
    }
}