using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using CommunityToolkit.Mvvm.Messaging;
using QrToPay.Models.Responses;

namespace QrToPay.Services
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class AuthService(IHttpClientFactory httpClientFactory)
    {

        private const string AuthStateKey = "AuthState";
        private const string UserIdKey = "UserId";
        private const string UserEmailKey = "UserEmail";
        private const string UserPhoneKey = "UserPhone";

        public async Task<bool> IsAuthenticatedAsync()
        {
            await Task.Delay(200);

            return Preferences.Get(AuthStateKey, false);
        }

        public async Task<LoginResult> Login(string emailPhone, string password)
        {
            var httpClient = httpClientFactory.CreateClient("ApiHttpClient");

            var loginData = new User();
            if (ValidationHelper.IsEmail(emailPhone))
            {
                loginData.Email = emailPhone;
            }
            else
            {
                loginData.PhoneNumber = emailPhone;
            }
            loginData.PasswordHash = password;
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("/auth/login/", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);

                if (user != null)
                {
                    Preferences.Set(AuthStateKey, true);
                    Preferences.Set(UserIdKey, user.UserID);

                    Preferences.Set(UserEmailKey, user.Email ?? "Brak");
                    Preferences.Set(UserPhoneKey, user.PhoneNumber ?? "Brak");

                    WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());

                    return new LoginResult { Success = true };
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContent);
                return new LoginResult { Success = false, ErrorMessage = errorResponse?.Message ?? "Błąd podczas logowania." };
            }

            return new LoginResult { Success = false, ErrorMessage = "Błąd podczas logowania." };
        }

        public void Logout()
        {
            Preferences.Remove(AuthStateKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(UserEmailKey);
            Preferences.Remove(UserPhoneKey);
        }

        public User GetUserData()
        {
            return new User()
            {
                UserID = Preferences.Get(UserIdKey, 0),
                Email = Preferences.Get(UserEmailKey, string.Empty),
                PhoneNumber = Preferences.Get(UserPhoneKey, string.Empty),
            };
        }
    }
}
