namespace QrToPay.Models.Common;

public sealed class City
{
    public required Guid ServiceId { get; init; }
    public required string CityName { get; init; }
}