namespace QrToPay.Api.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }
        public string? VerificationCode { get; set; }
        public decimal? AccountBalance { get; set; }
        public bool IsVerified { get; set; }

        public ICollection<UserTicket> UserTickets { get; set; } = [];
    }
}
