namespace QrToPay.Api.Features.SkiResorts.Prices;

public sealed class SkiResortPriceDto
{
    public required int SkiResortPriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
}