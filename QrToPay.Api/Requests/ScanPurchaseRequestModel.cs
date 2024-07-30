namespace QrToPay.Api.Requests;

public sealed class ScanPurchaseRequestModel
{
    public required int UserId { get; init; }
    public required string Type { get; init; }
    public required decimal Price { get; init; }
}