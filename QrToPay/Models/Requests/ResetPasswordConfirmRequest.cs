namespace QrToPay.Models.Requests;

public sealed class ResetPasswordConfirmRequest
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
    public required string NewPassword { get; init; }
}