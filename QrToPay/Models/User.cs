using QrToPay.Models.Responses;

namespace QrToPay.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public decimal? AccountBalance { get; set; }

        // Domyślny konstruktor
        public User() { }

        // Konstruktor mapujący z UserResponse
        public User(UserResponse response)
        {
            UserID = response.UserID;
            Email = response.Email;
            PhoneNumber = response.PhoneNumber;
            PasswordHash = response.PasswordHash;
            AccountBalance = response.AccountBalance;
        }
    }
}
