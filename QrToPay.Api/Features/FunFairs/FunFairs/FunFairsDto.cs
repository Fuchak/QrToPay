namespace QrToPay.Api.Features.FunFairs.FunFairs;

public sealed class FunFairsDto
{
    public required int FunFairId { get; init; }
    public required string ResortName { get; init; }
    public required string CityName { get; init; }
}