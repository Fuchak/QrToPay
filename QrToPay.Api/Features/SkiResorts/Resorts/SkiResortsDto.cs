namespace QrToPay.Api.Features.SkiResorts.Resorts;

public sealed class SkiResortsDto
{
    public required int SkiResortId { get; init; }
    public required string ResortName { get; init; }
    public required string CityName { get; init; }
}