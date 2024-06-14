namespace QrToPay.Api.DTOs
{
    //Login User
    public class UserDto
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public decimal? AccountBalance { get; set; }
    }
    //Reset Password
    public class UserExistRequest
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

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
        public required string Password { get; set; }
    }

    public class CreateUserResponse
    {
        public string? VerificationCode { get; set; }
    }

    public class VerifyRequest
    {
        public string? EmailOrPhone { get; set; }
        public string? VerificationCode { get; set; }
    }

    //User Balance
    public class UserBalanceRequest
    {
        public decimal? AccountBalance { get; set; }
    }
}
