namespace QrToPay.Api.DTOs
{
    //Login User
    public class UserDto
    {
        public int UserID { get; set; }
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public decimal? AccountBalance { get; set; }
    }
    //Check User Exist
    public class UserExistRequest
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
    //Reset User Password
    public class ResetPasswordRequest
    {
        public string? EmailOrPhone { get; set; }
        public string? VerificationCode { get; set; }
        public string? NewPassword { get; set; }
    }

    //Create User
    public class CreateUserRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string PasswordHash { get; set; }
    }
    //Pass Verification Code
    public class CreateUserResponse
    {
        public string? VerificationCode { get; set; }
    }
    //Verify registration
    public class VerifyRequest
    {
        public string? EmailOrPhone { get; set; }
        public string? VerificationCode { get; set; }
    }
    //Get User Balance
    public class UserBalanceRequest
    {
        public decimal? AccountBalance { get; set; }
    }

    public class TopUpRequest
    {
        public int UserID { get; set; }
        public decimal Amount { get; set; }
    }
}
