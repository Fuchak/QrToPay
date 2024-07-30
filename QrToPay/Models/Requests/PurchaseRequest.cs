namespace QrToPay.Models.Requests;

public sealed class PurchaseRequest
{
    public required int UserId { get; init; }
    public required string Type { get; init; }
    public required string AttractionName { get; init; }
    public required decimal Price { get; init; }
}