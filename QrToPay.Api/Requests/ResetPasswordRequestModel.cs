namespace QrToPay.Api.Requests;

public sealed class ResetPasswordRequestModel
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
    public required string NewPassword { get; init; }
}