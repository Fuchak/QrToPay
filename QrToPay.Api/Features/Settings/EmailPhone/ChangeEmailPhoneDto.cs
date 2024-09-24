namespace QrToPay.Api.Features.Settings.EmailPhone;

public sealed class ChangeEmailPhoneDto
{
    public required string VerificationCode { get; init; }
}
