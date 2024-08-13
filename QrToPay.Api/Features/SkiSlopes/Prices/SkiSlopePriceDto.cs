namespace QrToPay.Api.Features.SkiSlopes.Prices;

public sealed class SkiSlopePriceDto
{
    public required int SkiSlopePriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
}