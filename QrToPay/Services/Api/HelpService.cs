using System.Net.Http.Headers;
using System.Net.Http.Json;
using QrToPay.Helpers;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;

namespace QrToPay.Services.Api;
public class HelpService
{
    private readonly HttpClientHelper _httpClientHelper;

    public HelpService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<ServiceResult<string>> SubmitHelpFormAsync(HelpFormRequest request)
    {
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

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