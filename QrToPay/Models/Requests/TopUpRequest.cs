namespace QrToPay.Models.Requests;

public sealed class TopUpRequest
{
    public required decimal Amount { get; init; }
}