using System.Net.Http.Json;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;

namespace QrToPay.Services.Api;
public class QrCodeService
{
    private readonly HttpClientHelper _httpClientHelper;

    public QrCodeService(HttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }
    public async Task<ServiceResult<object>> ActivateQrCodeAsync(string token)
    {
        var requestModel = new
        {
            Token = token,
        };
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/Tickets/activate", requestModel);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<object>.Success();
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Aktywacja nie powiodła się.";
                return ServiceResult<object>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<object>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<object>> DeactivateQrCodeAsync(string token)
    {//Todo zrobić model do tego xd
        var requestModel = new
        {
            Token = token,
        };
        try
        {
            HttpClient client = await _httpClientHelper.CreateAuthenticatedClientAsync();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/Tickets/deactivate", requestModel);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<object>.Success();
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Dezaktywacja nie powiodła się.";
                return ServiceResult<object>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<object>.Failure(HttpError.HandleError(ex));
        }
    }
}