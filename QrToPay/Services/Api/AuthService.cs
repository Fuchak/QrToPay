using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Models.Common;
using System.Diagnostics;

namespace QrToPay.Services.Api
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private const string AuthStateKey = "AuthState";
        private const string UserIdKey = "UserId";
        private const string UserEmailKey = "UserEmail";
        private const string UserPhoneKey = "UserPhone";

        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(200);
            return Preferences.Get(AuthStateKey, false);
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
                        Preferences.Set(AuthStateKey, true);
                        Preferences.Set(UserIdKey, userResponse.UserId);
                        Preferences.Set(UserEmailKey, userResponse.Email ?? "Brak");
                        Preferences.Set(UserPhoneKey, userResponse.PhoneNumber ?? "Brak");

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

        public void Logout()
        {
            Preferences.Remove(AuthStateKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(UserEmailKey);
            Preferences.Remove(UserPhoneKey);
        }
    }
}