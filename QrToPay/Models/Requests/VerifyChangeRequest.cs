using QrToPay.Models.Enums;

namespace QrToPay.Models.Requests;
public sealed class VerifyChangeRequest
{
    public required int UserId { get; init; }
    public required string VerificationCode { get; init; }
    public required ChangeType ChangeType { get; init; }
}