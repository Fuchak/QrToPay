namespace QrToPay.Models.Requests;

public sealed class TopUpRequest
{
    public required int UserId { get; init; }
    public required decimal Amount { get; init; }
}