namespace QrToPay.Models.Responses;

public sealed class SkiResortsResponse
{
    public required int SkiResortId { get; init; }
    public required string ResortName { get; init; }
    public required string CityName { get; init; }
}