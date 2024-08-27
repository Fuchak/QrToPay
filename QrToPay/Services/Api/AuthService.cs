using System.Text;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Models.Common;

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

        public async Task<ApiResponse> LoginAsync(string emailPhone, string password)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("ApiHttpClient");

                LoginUserRequest loginData = ValidationHelper.IsEmail(emailPhone)
                    ? new LoginUserRequest { Email = emailPhone, PasswordHash = password }
                    : new LoginUserRequest { PhoneNumber = emailPhone, PasswordHash = password };

                StringContent content = new(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("/api/Auth/login/", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    UserResponse? userResponse = JsonConvert.DeserializeObject<UserResponse>(result);

                    if (userResponse != null)
                    {
                        Preferences.Set(AuthStateKey, true);
                        Preferences.Set(UserIdKey, userResponse.UserId);
                        Preferences.Set(UserEmailKey, userResponse.Email ?? "Brak");
                        Preferences.Set(UserPhoneKey, userResponse.PhoneNumber ?? "Brak");

                        return ApiResponse.SuccessResponse("Login successful.");
                    }
                    else
                    {
                        return ApiResponse.ErrorResponse("Błąd: Nie udało się przetworzyć odpowiedzi serwera.");
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return ApiResponse.ErrorResponse(errorContent);
                }
            }
            catch (HttpRequestException httpEx)
            {
                return ApiResponse.ErrorResponse(HttpError.HandleHttpError(httpEx));
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResponse("Wystąpił nieoczekiwany błąd: " + ex.Message);
            }
        }

        public void Logout()
        {
            Preferences.Remove(AuthStateKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(UserEmailKey);
            Preferences.Remove(UserPhoneKey);
        }

        public UserPreferences GetUserData()
        {
            return new UserPreferences()
            {
                UserId = Preferences.Get(UserIdKey, 0),
                Email = Preferences.Get(UserEmailKey, string.Empty),
                PhoneNumber = Preferences.Get(UserPhoneKey, string.Empty),
            };
        }
    }
}