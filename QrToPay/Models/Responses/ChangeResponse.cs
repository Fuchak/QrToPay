namespace QrToPay.Models.Responses;

public sealed class ChangeResponse //TODO zmienić na verificationCodeResponse
{
    public required string VerificationCode { get; init; }
}