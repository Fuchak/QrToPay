namespace QrToPay.Models.Responses;

public sealed class ChangeResponse
{
    public required string VerificationCode { get; init; }
}