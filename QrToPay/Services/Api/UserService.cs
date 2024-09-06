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

    public async Task<ServiceResult<object>> VerifyUserAsync(VerifyCreateUserRequest request)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Register/verify", request);

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

    public async Task<ServiceResult<object>> ResetPasswordAsync(ResetPasswordConfirmRequest request)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Auth/resetPassword", request);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult<object>.Success();
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                return ServiceResult<object>.Failure(errorMessage ?? "Błąd podczas resetowania hasła");
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<object>.Failure(HttpError.HandleError(ex));
        }
    }

    public async Task<ServiceResult<ChangeResponse>> CheckAccountAsync(ResetPasswordRequest request)
    {
        HttpClient client = _httpClientFactory.CreateClient("ApiHttpClient");

        try
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Auth/checkAccount", request);
            if (response.IsSuccessStatusCode)
            {
                ChangeResponse? changeResponse = await response.Content.ReadFromJsonAsync<ChangeResponse>();
                if (changeResponse != null)
                {
                    return ServiceResult<ChangeResponse>.Success(changeResponse);
                }
                else
                {
                    return ServiceResult<ChangeResponse>.Failure("Błąd podczas odczytywania kodu weryfikacyjnego.");
                }
            }
            else
            {
                string errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response)
                    ?? "Błąd podczas resetowania hasła. Spróbuj ponownie.";
                return ServiceResult<ChangeResponse>.Failure(errorMessage);
            }
        }
        catch (Exception ex)
        {
            return ServiceResult<ChangeResponse>.Failure(HttpError.HandleError(ex));
        }
    }
}
