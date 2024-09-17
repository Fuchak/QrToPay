namespace QrToPay.Api.Features.Auth.CheckAccount;

public sealed class CheckAccountDto
{
    public required string VerificationCode { get; init; }
}