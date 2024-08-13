using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Settings.Verify;
public sealed class VerifyRequestModel
{
    public required int UserId { get; init; }
    public required string VerificationCode { get; init; }
    public required ChangeType ChangeType { get; init; }
}