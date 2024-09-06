using QrToPay.Models.Enums;

namespace QrToPay.Models.Requests;

public sealed class ResetPasswordRequest
{
    public required string Contact { get; init; }
    public required ChangeType ChangeType { get; init; }
}