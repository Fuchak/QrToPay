using QrToPay.Models.Enums;

namespace QrToPay.Models.Requests;

//zmiana dla zmiany numeru telefonu i email xdd
public sealed class ChangeRequest
{
    public required int UserId { get; init; }
    public required string Password { get; init; }
    public required string NewValue { get; init; }
    public required ChangeType ChangeType { get; init; }
}