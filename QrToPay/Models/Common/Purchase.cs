namespace QrToPay.Models.Common;

public sealed class Purchase
{
    public required string Type { get; init; }
    public required string AttractionName { get; init; }
    public required decimal Price { get; init; }
}