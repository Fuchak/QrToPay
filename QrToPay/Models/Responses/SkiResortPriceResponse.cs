namespace QrToPay.Models.Responses;

public sealed class SkiResortPriceResponse
{
    //TODO api nie zwraca lift name a mogłoby
    public int SkiResortPriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
    //public string? LiftName { get; init; }
}