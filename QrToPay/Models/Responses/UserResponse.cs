namespace QrToPay.Models.Responses
{
    public class UserResponse
    {
        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public decimal? AccountBalance { get; set; }
    }
    
    public class CreateUserResponse
    {
        public string? EmailOrPhone { get; set; }
        public string? VerificationCode { get; set; }
    }



    //Poniżej dodane żeby tylko działało by pokazać że baza połączona pewnie to trzeba będzie przenosić
    public class TopUpResponse
    {
        public int UserID { get; set; }
        public decimal Amount { get; set; }
    }
    //Tutaj settingsy w sumie też trzeba będzie je gdzieś przenieść ale na razie zostawiam tutaj żeby tylko działało
    public class ChangeEmailResponse
    {
        public string? VerificationCode { get; set; }
    }
    public class ChangePhoneResponse
    {
        public string? VerificationCode { get; set; }
    }
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
    //Tutaj jest dla pomocy
    public class HelpFormRequest
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } // Domyślnie "Nowe"
    }
}
