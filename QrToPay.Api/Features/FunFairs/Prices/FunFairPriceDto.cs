namespace QrToPay.Api.Features.FunFairs.Prices;

public sealed class FunFairPriceDto
{
    public required int FunFairPriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
}