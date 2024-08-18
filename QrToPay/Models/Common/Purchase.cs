namespace QrToPay.Models.Common;

public sealed class Purchase
{
    public required string ServiceName { get; init; }
    public required Guid ServiceId { get; init; }
    public required string AttractionName { get; init; }
    public required decimal Price { get; init; }
}