namespace QrToPay.Api.Features.SkiSlopes.Slopes;

public sealed class SkiSlopeDto
{
    public required int SkiResortId { get; init; }
    public required string ResortName { get; init; }
    public required string CityName { get; init; }
}