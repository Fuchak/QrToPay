using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;

public class SkiResortService
{
    private readonly HttpClientHelper _httpClientHelper;

    public SkiResortService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<ServiceResult<List<SkiResortPriceResponse>>> GetSkiResortPricesAsync(int skiResortId)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

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

    public async Task<ServiceResult<List<SkiResortsResponse>>> GetSkiResortsAsync(string cityName)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.GetAsync($"/api/SkiResorts/city?cityName={cityName}");

            if (response.IsSuccessStatusCode)
            {
                List<SkiResortsResponse>? skiResorts = await response.Content.ReadFromJsonAsync<List<SkiResortsResponse>>();
                if (skiResorts != null)
                {
                    return ServiceResult<List<SkiResortsResponse>>.Success(skiResorts);
                }
                return ServiceResult<List<SkiResortsResponse>>.Failure("Nie udało się pobrać danych ze stoku narciarskiego.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas pobierania danych ze stoku narciarskiego.";
                return ServiceResult<List<SkiResortsResponse>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<SkiResortsResponse>>.Failure(HttpError.HandleError(ex));
        }
    }
}