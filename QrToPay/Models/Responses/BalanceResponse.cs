namespace QrToPay.Models.Responses;

public sealed class BalanceResponse
{
    public required decimal AccountBalance { get; init; }
}