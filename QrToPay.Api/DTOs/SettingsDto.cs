namespace QrToPay.Api.DTOs
{
    public class ChangeEmailRequest
    {
        public int UserId { get; set; }
        public string? NewEmail { get; set; }
        public string? Password { get; set; }
    }

    public class VerifyEmailRequest
    {
        public int UserId { get; set; }
        public string? VerificationCode { get; set; }
    }

    public class ChangePhoneRequest
    {
        public int UserId { get; set; }
        public string? NewPhoneNumber { get; set; }
        public string? Password { get; set; }
    }

    public class VerifyPhoneRequest
    {
        public int UserId { get; set; }
        public string? VerificationCode { get; set; }
    }

    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
    }
}
