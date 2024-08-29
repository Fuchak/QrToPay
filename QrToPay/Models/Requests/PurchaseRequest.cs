using QrToPay.Models.Common;

namespace QrToPay.Models.Requests;

public sealed class PurchaseRequest
{
    public required int UserId { get; init; }
    public required Purchase Purchase { get; init; }
}