namespace QrToPay.Models.Requests;
public sealed class VerifyCreateUserRequest
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
}