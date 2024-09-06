using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace QrToPay.Services.Api;

public class CityService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CityService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ServiceResult<List<City>>> GetCitiesAsync(int serviceType)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Cities?serviceType={serviceType}");

            if (response.IsSuccessStatusCode)
            {
                List<City>? cities = await response.Content.ReadFromJsonAsync<List<City>>();
                if (cities != null)
                {
                    return ServiceResult<List<City>>.Success(cities);
                }
                return ServiceResult<List<City>>.Failure("Błąd podczas odczytywania listy miast.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<List<City>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<City>>.Failure(HttpError.HandleError(ex));
        }
    }
}