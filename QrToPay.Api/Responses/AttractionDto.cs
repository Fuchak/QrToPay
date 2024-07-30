namespace QrToPay.Api.Responses;

public sealed class AttractionDto
{
    public required string Type { get; init; }
    public required string AttractionName { get; init; }
    public required double Price { get; init; }
}