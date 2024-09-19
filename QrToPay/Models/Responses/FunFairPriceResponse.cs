namespace QrToPay.Models.Responses;

public sealed class FunFairPriceResponse
{
    //TODO api nie zwraca atraction name a mogłoby
    public int FunFairPriceId { get; init; }
    public required int Tokens { get; init; }
    public required decimal Price { get; init; }
    //public string? atraction { get; init; }
}