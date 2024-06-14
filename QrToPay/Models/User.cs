namespace QrToPay.Models
{
    //Login user
    public class User
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public decimal? AccountBalance { get; set; }
    }
    //Register user
    public class CreateUserResponse
    {
        public string? EmailOrPhone { get; set; }
        public string? VerificationCode { get; set; }
    }
    //Error hanlder
    public class ErrorResponse
    {
        public string? Message { get; set; }
    }

}
