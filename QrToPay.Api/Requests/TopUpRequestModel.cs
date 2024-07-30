namespace QrToPay.Api.Requests;

public sealed class TopUpRequestModel
{
    public required int UserId { get; init; }
    public required decimal Amount { get; init; }
}