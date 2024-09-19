namespace QrToPay.Models.Responses;

public sealed class FunFairsResponse
{
    public required int FunFairId { get; init; }
    public required string ResortName { get; init; }
    public required string CityName { get; init; }
}