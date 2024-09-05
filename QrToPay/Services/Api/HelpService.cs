using System.Net.Http.Json;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;
public class HelpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HelpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ServiceResult<string>> SubmitHelpFormAsync(HelpFormRequest request)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Support", request);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<string>.Success("Twoje zgłoszenie zostało wysłane.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<string>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<string>.Failure(HttpError.HandleError(ex));
        }
    }
}