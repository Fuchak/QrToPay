using System.Text;
using Newtonsoft.Json;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;
using QrToPay.Models.Requests;
using QrToPay.Models.Common;

namespace QrToPay.Services
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

        public async Task<ApiResponse> Login(string emailPhone, string password)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("ApiHttpClient");

            LoginUserRequest loginData = ValidationHelper.IsEmail(emailPhone)
                ? new LoginUserRequest { Email = emailPhone, PasswordHash = password }
                : new LoginUserRequest { PhoneNumber = emailPhone, PasswordHash = password };

            StringContent content = new (JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

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

                    WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());

                    return ApiResponse.SuccessResponse("Login successful.");
                }
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                ApiResponse? errorResponse = JsonConvert.DeserializeObject<ApiResponse>(errorContent);

                return ApiResponse.ErrorResponse(errorResponse?.Message ?? "Błąd podczas logowania.");
            }

            return ApiResponse.ErrorResponse("Błąd podczas logowania.");
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