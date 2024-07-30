namespace QrToPay.Api.Requests;

public sealed class VerifyCreateUserRequestModel
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
}