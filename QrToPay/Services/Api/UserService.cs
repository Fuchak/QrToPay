using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using QrToPay.Models.Requests;
using QrToPay.Models.Responses;
using QrToPay.Helpers;
using System.Diagnostics;

namespace QrToPay.Services.Api;

public class UserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ServiceResult<CreateUserResponse>> RegisterUserAsync(CreateUserRequest request)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Register/register", request);

            if (response.IsSuccessStatusCode)
            {
                CreateUserResponse? registerResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
                if (registerResponse != null)
                {
                    return ServiceResult<CreateUserResponse>.Success(registerResponse);
                }
                return ServiceResult<CreateUserResponse>.Failure("Błąd podczas odczytywania odpowiedzi serwera.");
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<CreateUserResponse>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<CreateUserResponse>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<object>> VerifyUserAsync(VerifyCreateUserRequest verifyRequest)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Register/verify", verifyRequest);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<object>.Success();
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<object>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<object>.Failure(HttpError.HandleError(ex));
        }
    }
}
