namespace QrToPay.Api.Features.Cities;
public sealed class CitiesDto
{
    public required Guid ServiceId { get; init; }
    public required string CityName { get; init; }
}