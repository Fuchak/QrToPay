using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Models.Common;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using QrToPay.Services.Local;
using QrToPay.Messages;

namespace QrToPay.Services.Api
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public static async Task<bool> IsAuthenticatedAsync()
        {
            //await Task.Delay(200);
            // Sprawdzenie, czy token istnieje w SecureStorage
            var token = await SecureStorage.GetAsync(SecureStorageConst.AuthToken);
            await UserIdentifierService.GetOrCreateUserUUIDAsync();

            if (string.IsNullOrEmpty(token) || IsTokenExpired(token))
            {
                // Wysłanie wiadomości o potrzebie wylogowania
                WeakReferenceMessenger.Default.Send(new UserLogoutRequestMessage("Token expired"));
                return false;
            }
            // Token jest ważny
            return true;
        }

        public async Task<ServiceResult<UserResponse>> LoginAsync(string emailPhone, string password)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("ApiHttpClient");

                LoginUserRequest loginData = ValidationHelper.IsEmail(emailPhone)
                    ? new LoginUserRequest { Email = emailPhone, PasswordHash = password }
                    : new LoginUserRequest { PhoneNumber = emailPhone, PasswordHash = password };

                StringContent content = new(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("/api/Auth/login/", content);

                if (response.IsSuccessStatusCode)
                {
                    UserResponse? userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

                    if (userResponse != null)
                    {
                        await SecureStorage.SetAsync(SecureStorageConst.AuthToken, userResponse.Token);
                        await SecureStorage.SetAsync(SecureStorageConst.UserEmail, userResponse.Email!);
                        await SecureStorage.SetAsync(SecureStorageConst.UserPhone, userResponse.PhoneNumber!);

                        return ServiceResult<UserResponse>.Success(userResponse);
                    }
                    else
                    {
                        return ServiceResult<UserResponse>.Failure("Nie udało się przetworzyć odpowiedzi serwera.");
                    }
                }
                else
                {
                    // Użycie JsonErrorExtractor do wyciągnięcia wiadomości błędu
                    var errorMessage = await JsonErrorExtractor.ExtractErrorMessageAsync(response);
                    return ServiceResult<UserResponse>.Failure(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<UserResponse>.Failure(HttpError.HandleError(ex));
            }
        }

        private static bool IsTokenExpired(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                if (!jwtTokenHandler.CanReadToken(token))
                    return true;

                var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                var expirationDate = jwtToken.ValidTo;

                return expirationDate < DateTime.UtcNow;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}