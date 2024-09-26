using System.Net.Http.Json;
using QrToPay.Models.Common;
using QrToPay.Services.Api;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using QrToPay.Helpers;

namespace QrToPay.Services.Api;

public class CityService
{
    private readonly HttpClientHelper _httpClientHelper;

    public CityService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<ServiceResult<List<City>>> GetCitiesAsync(int serviceType)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

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
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Nie udało się pobrać listy miast.";

                return ServiceResult<List<City>>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<List<City>>.Failure(HttpError.HandleError(ex));
        }
    }
}